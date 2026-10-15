import tkinter as tk
from tkinter import ttk, scrolledtext, messagebox
import random
import time


# ------------------------------------------------------------
# Ручная реализация Base64 (без импорта base64)
# ------------------------------------------------------------
class Base64Manual:
    """Ручное кодирование/декодирование Base64"""

    @staticmethod
    def encode(text):
        """Кодирование строки в Base64 (ручная реализация)"""
        # Таблица Base64
        b64_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"

        # Преобразуем строку в байты
        if isinstance(text, str):
            bytes_data = text.encode('utf-8')
        else:
            bytes_data = text

        result = []
        # Обрабатываем по 3 байта (24 бита)
        for i in range(0, len(bytes_data), 3):
            chunk = bytes_data[i:i + 3]

            # Формируем 24-битное число
            n = 0
            for j, byte in enumerate(chunk):
                n |= byte << (16 - 8 * j)

            # Количество бит в последнем блоке
            pad = 3 - len(chunk)

            # Разбиваем на 4 группы по 6 бит
            for j in range(4 - pad):
                index = (n >> (18 - 6 * j)) & 0x3F
                result.append(b64_chars[index])

            # Добавляем padding
            for _ in range(pad):
                result.append('=')

        return ''.join(result)

    @staticmethod
    def decode(b64_str):
        """Декодирование Base64 строки (ручная реализация)"""
        b64_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"
        char_to_index = {ch: i for i, ch in enumerate(b64_chars)}

        # Убираем padding
        b64_str = b64_str.rstrip('=')

        result_bytes = []
        # Обрабатываем по 4 символа
        for i in range(0, len(b64_str), 4):
            chunk = b64_str[i:i + 4]

            # Формируем 24-битное число
            n = 0
            for j, ch in enumerate(chunk):
                if ch == '=':
                    break
                n |= char_to_index[ch] << (18 - 6 * j)

            # Извлекаем байты
            for j in range(len(chunk) - 1):
                byte = (n >> (16 - 8 * j)) & 0xFF
                result_bytes.append(byte)

        return bytes(result_bytes).decode('utf-8', errors='replace')


# ------------------------------------------------------------
# Класс криптосистемы Меркла-Хеллмана
# ------------------------------------------------------------
class MerkleHellman:
    def __init__(self, n, z=8):
        """
        n: число членов последовательности (длина ключа в битах)
        z: основание кодировки (6 для Base64, 8 для ASCII)
        """
        self.n = n
        self.z = z
        self.super_seq = []  # секретный сверхвозрастающий ключ
        self.public_seq = []  # открытый ключ
        self.q = 0  # модуль (q > sum(super_seq))
        self.r = 0  # множитель (взаимно прост с q)
        self.r_inv = 0  # обратное к r по модулю q

    def gcd_extended(self, a, b):
        """Расширенный алгоритм Евклида (обратный элемент по модулю)"""
        if a == 0:
            return (b, 0, 1)
        else:
            g, y, x = self.gcd_extended(b % a, a)
            return (g, x - (b // a) * y, y)

    def mod_inverse(self, a, m):
        """Обратный элемент a mod m"""
        g, x, _ = self.gcd_extended(a, m)
        if g != 1:
            raise Exception("Обратный элемент не существует")
        else:
            return x % m

    def generate_super_seq(self):
        """Генерация сверхвозрастающей последовательности.
           Старший член — 100-битное число (~2^100)."""
        self.super_seq = []
        total = 0
        for i in range(self.n):
            if i == self.n - 1:
                # последний член ~ 2^100
                lower = max(total + 1, 2 ** 99)
                upper = 2 ** 100 + 10 ** 15
            else:
                lower = total + 1
                upper = total + random.randint(10, 1000) + 1000
            val = random.randint(lower, upper)
            self.super_seq.append(val)
            total += val
        return self.super_seq

    def compute_public_key(self):
        """Вычисление нормальной (открытой) последовательности."""
        total = sum(self.super_seq)
        self.q = total + random.randint(total // 2, total)
        # Поиск r взаимно простого с q
        while True:
            r_candidate = random.randint(2, self.q - 1)
            if self.gcd_extended(r_candidate, self.q)[0] == 1:
                self.r = r_candidate
                break
        self.r_inv = self.mod_inverse(self.r, self.q)
        self.public_seq = [(w * self.r) % self.q for w in self.super_seq]
        return self.public_seq

    def text_to_numbers(self, text):
        """Преобразование текста в список чисел по таблице z"""
        numbers = []
        if self.z == 6:  # Base64 таблица
            # Используем ручную реализацию Base64
            b64_str = Base64Manual.encode(text)
            for ch in b64_str:
                if 'A' <= ch <= 'Z':
                    numbers.append(ord(ch) - ord('A'))
                elif 'a' <= ch <= 'z':
                    numbers.append(ord(ch) - ord('a') + 26)
                elif '0' <= ch <= '9':
                    numbers.append(ord(ch) - ord('0') + 52)
                elif ch == '+':
                    numbers.append(62)
                elif ch == '/':
                    numbers.append(63)
                else:
                    numbers.append(0)
        elif self.z == 8:  # ASCII таблица (0..255)
            for byte in text.encode('utf-8'):
                numbers.append(byte)
        return numbers

    def numbers_to_text(self, numbers):
        """Обратное преобразование из чисел в текст"""
        if self.z == 6:
            chars = []
            for val in numbers:
                if 0 <= val <= 25:
                    chars.append(chr(val + ord('A')))
                elif 26 <= val <= 51:
                    chars.append(chr(val - 26 + ord('a')))
                elif 52 <= val <= 61:
                    chars.append(chr(val - 52 + ord('0')))
                elif val == 62:
                    chars.append('+')
                elif val == 63:
                    chars.append('/')
                else:
                    chars.append('=')
            b64_str = ''.join(chars)
            # Ручное декодирование Base64
            try:
                return Base64Manual.decode(b64_str)
            except:
                return "[Ошибка декодирования Base64]"
        elif self.z == 8:
            return bytes(numbers).decode('utf-8', errors='replace')
        return ""

    def encrypt_block(self, block_bits):
        """Шифрование одного блока (битовый вектор длины n) -> сумма"""
        if len(block_bits) != self.n:
            raise ValueError("Длина блока не совпадает с n")
        total = 0
        for i, bit in enumerate(block_bits):
            if bit == 1:
                total += self.public_seq[i]
        return total

    def decrypt_sum(self, cipher_sum):
        """Расшифрование суммы -> битовый вектор"""
        cp = (cipher_sum * self.r_inv) % self.q
        bits = [0] * self.n
        remaining = cp
        for i in range(self.n - 1, -1, -1):
            if remaining >= self.super_seq[i]:
                bits[i] = 1
                remaining -= self.super_seq[i]
        return bits

    def encrypt_message(self, plaintext):
        """Шифрование полного сообщения"""
        numbers = self.text_to_numbers(plaintext)
        cipher_blocks = []
        for num in numbers:
            if num >= (1 << self.n):
                # Если число не влезает, используем максимальное значение
                num = (1 << self.n) - 1
            bits = [(num >> (self.n - 1 - i)) & 1 for i in range(self.n)]
            total = self.encrypt_block(bits)
            cipher_blocks.append(total)
        return cipher_blocks

    def decrypt_message(self, cipher_blocks):
        """Расшифрование списка сумм обратно в текст"""
        numbers = []
        for val in cipher_blocks:
            bits = self.decrypt_sum(val)
            num = 0
            for b in bits:
                num = (num << 1) | b
            numbers.append(num)
        return self.numbers_to_text(numbers)

# ------------------------------------------------------------
# GUI приложение
# ------------------------------------------------------------
class App:
    def __init__(self, root):
        self.root = root
        self.root.title("Асимметричное шифрование: криптосистема Меркла-Хеллмана")
        self.root.geometry("900x700")

        self.crypto = None
        self.n = 10
        self.z = 8
        self.last_cipher = None

        self.create_widgets()

    def create_widgets(self):
        # Панель параметров
        frame_params = ttk.LabelFrame(self.root, text="Параметры ключа", padding=10)
        frame_params.pack(fill="x", padx=10, pady=5)

        ttk.Label(frame_params, text="Длина ключа n (число членов):").grid(row=0, column=0, sticky="w", padx=5)
        self.n_entry = ttk.Entry(frame_params, width=10)
        self.n_entry.grid(row=0, column=1, padx=5)
        self.n_entry.insert(0, "10")

        ttk.Label(frame_params, text="Таблица кодировки:").grid(row=0, column=2, sticky="w", padx=5)
        self.z_var = tk.StringVar(value="ASCII (8 бит)")
        z_combo = ttk.Combobox(frame_params, textvariable=self.z_var,
                               values=["ASCII (8 бит)", "Base64 (6 бит)"], width=20)
        z_combo.grid(row=0, column=3, padx=5)
        z_combo.bind("<<ComboboxSelected>>", self.on_z_change)

        self.gen_btn = ttk.Button(frame_params, text="Сгенерировать ключи", command=self.generate_keys)
        self.gen_btn.grid(row=0, column=4, padx=10)

        # Отображение ключей
        frame_keys = ttk.LabelFrame(self.root, text="Ключи", padding=10)
        frame_keys.pack(fill="x", padx=10, pady=5)

        self.secret_key_text = scrolledtext.ScrolledText(frame_keys, height=4, width=100, wrap=tk.WORD)
        self.secret_key_text.pack(fill="x", pady=2)
        self.public_key_text = scrolledtext.ScrolledText(frame_keys, height=4, width=100, wrap=tk.WORD)
        self.public_key_text.pack(fill="x", pady=2)

        # Ввод сообщения
        frame_input = ttk.LabelFrame(self.root, text="Исходное сообщение (ФИО)", padding=10)
        frame_input.pack(fill="x", padx=10, pady=5)

        self.message_entry = ttk.Entry(frame_input, width=80)
        self.message_entry.pack(fill="x", padx=5, pady=5)
        self.message_entry.insert(0, "Иванов Иван Иванович")

        # Кнопки действий
        frame_buttons = ttk.Frame(self.root)
        frame_buttons.pack(pady=10)

        self.encrypt_btn = ttk.Button(frame_buttons, text="Зашифровать",
                                      command=self.encrypt_message, state="disabled")
        self.encrypt_btn.pack(side="left", padx=10)

        self.decrypt_btn = ttk.Button(frame_buttons, text="Расшифровать",
                                      command=self.decrypt_message, state="disabled")
        self.decrypt_btn.pack(side="left", padx=10)

        # Результаты
        frame_result = ttk.LabelFrame(self.root, text="Результаты", padding=10)
        frame_result.pack(fill="both", expand=True, padx=10, pady=5)
        ttk.Label(frame_result, text="Шифротекст (суммы):").pack(anchor="w")
        self.cipher_text = scrolledtext.ScrolledText(frame_result, height=5, width=100)
        self.cipher_text.pack(fill="x", pady=2)

        ttk.Label(frame_result, text="Расшифрованное сообщение:").pack(anchor="w")
        self.decrypted_text = scrolledtext.ScrolledText(frame_result, height=5, width=100)
        self.decrypted_text.pack(fill="x", pady=2)

        ttk.Label(frame_result, text="Время выполнения (сек):").pack(anchor="w")
        self.time_label = ttk.Label(frame_result, text="—")
        self.time_label.pack(anchor="w", padx=5)

    def on_z_change(self, event=None):
        if self.z_var.get() == "ASCII (8 бит)":
            self.z = 8
        else:
            self.z = 6

    def generate_keys(self):
        try:
            self.n = int(self.n_entry.get())
            if self.n < 6:
                messagebox.showwarning("Ошибка", "n должно быть не менее 6")
                return
            self.on_z_change()
            self.crypto = MerkleHellman(self.n, self.z)
            self.crypto.generate_super_seq()
            self.crypto.compute_public_key()

            self.secret_key_text.delete(1.0, tk.END)
            self.secret_key_text.insert(tk.END, f"Сверхвозрастающая последовательность (n={self.n}):\n")
            self.secret_key_text.insert(tk.END, ", ".join(map(str, self.crypto.super_seq)))
            self.secret_key_text.insert(tk.END,
                                        f"\n\nq = {self.crypto.q}\nr = {self.crypto.r}\nr^-1 = {self.crypto.r_inv}")

            self.public_key_text.delete(1.0, tk.END)
            self.public_key_text.insert(tk.END, f"Открытая последовательность (n={self.n}):\n")
            self.public_key_text.insert(tk.END, ", ".join(map(str, self.crypto.public_seq)))

            self.encrypt_btn.config(state="normal")
            self.decrypt_btn.config(state="normal")
            messagebox.showinfo("Генерация ключей", f"Ключи сгенерированы. n={self.n}, кодировка={self.z}-бит")
        except Exception as e:
            messagebox.showerror("Ошибка", str(e))

    def encrypt_message(self):
        if not self.crypto:
            messagebox.showwarning("Внимание", "Сначала сгенерируйте ключи")
            return
        plain = self.message_entry.get().strip()
        if not plain:
            messagebox.showwarning("Внимание", "Введите сообщение")
            return

        start = time.time()
        try:
            cipher_blocks = self.crypto.encrypt_message(plain)
            end = time.time()
            enc_time = end - start

            self.cipher_text.delete(1.0, tk.END)
            self.cipher_text.insert(tk.END, ", ".join(map(str, cipher_blocks)))
            self.time_label.config(text=f"Зашифрование: {enc_time:.6f} сек | Расшифрование: —")
            self.last_cipher = cipher_blocks
            messagebox.showinfo("Готово", f"Сообщение зашифровано за {enc_time:.6f} сек")
        except Exception as e:
            messagebox.showerror("Ошибка шифрования", str(e))

    def decrypt_message(self):
        if not self.crypto or self.last_cipher is None:
            messagebox.showwarning("Внимание", "Нет зашифрованного сообщения")
            return

        start = time.time()
        try:
            decrypted = self.crypto.decrypt_message(self.last_cipher)
            end = time.time()
            dec_time = end - start

            self.decrypted_text.delete(1.0, tk.END)
            self.decrypted_text.insert(tk.END, decrypted)

            old_text = self.time_label.cget("text")
            if "Зашифрование" in old_text:
                self.time_label.config(text=f"{old_text.split('|')[0]} | Расшифрование: {dec_time:.6f} сек")
            else:
                self.time_label.config(text=f"Расшифрование: {dec_time:.6f} сек")
            messagebox.showinfo("Готово", f"Сообщение расшифровано за {dec_time:.6f} сек")
        except Exception as e:
            messagebox.showerror("Ошибка расшифрования", str(e))
# ------------------------------------------------------------
if __name__ == "__main__":
    root = tk.Tk()
    app = App(root)
    root.mainloop()