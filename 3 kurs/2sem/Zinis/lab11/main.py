import tkinter as tk
from tkinter import scrolledtext, messagebox, simpledialog

p = 751
a = -1
b = 1

def extended_gcd(a, b):
    """Расширенный алгоритм Евклида. Возвращает (gcd, x, y) такие, что a*x + b*y = gcd"""
    if a == 0:
        return b, 0, 1
    gcd, x1, y1 = extended_gcd(b % a, a)
    x = y1 - (b // a) * x1
    y = x1
    return gcd, x, y

def inverse_mod(k, mod):
    """Возвращает обратный элемент k по модулю mod (k^{-1} mod mod)"""
    k = k % mod  # Нормализация
    gcd, x, _ = extended_gcd(k, mod)
    if gcd != 1:
        raise ValueError(f"Обратного элемента не существует для {k} по модулю {mod}")
    return x % mod

def is_on_curve(x, y, a_curve, b_curve, mod):
    """Проверяет, принадлежит ли точка (x, y) эллиптической кривой"""
    if x is None and y is None:  # точка O
        return True
    return (y * y) % mod == (pow(x, 3, mod) + a_curve * x + b_curve) % mod

def point_add(P, Q, a_curve, mod):
    """
    Сложение двух точек P и Q на эллиптической кривой.
    Точки передаются как кортежи (x, y) или как (None, None) для точки O.
    Возвращает точку R = P + Q.
    """
    # Если одна из точек - O, возвращаем другую
    if P == (None, None):
        return Q
    if Q == (None, None):
        return P

    x1, y1 = P
    x2, y2 = Q

    # Если x1 == x2
    if x1 == x2:
        # Если y1 == -y2 (mod mod) или y1 == y2 == 0, сумма - O
        if (y1 + y2) % mod == 0:
            return (None, None)  # точка O
        # Если P == Q, выполняем удвоение
        if y1 == y2:
            if y1 == 0:  # касательная вертикальна
                return (None, None)
            # λ = (3x^2 + a) / 2y
            lam = ((3 * x1 * x1 + a_curve) * inverse_mod((2 * y1) % mod, mod)) % mod
        else:
            # Случай x1 == x2, но y1 != y2 и y1 + y2 != 0 не должен происходить
            return (None, None)
    else:
        # λ = (y2 - y1) / (x2 - x1)
        dx = (x2 - x1) % mod
        dy = (y2 - y1) % mod
        lam = (dy * inverse_mod(dx, mod)) % mod

    x3 = (lam * lam - x1 - x2) % mod
    y3 = (lam * (x1 - x3) - y1) % mod
    return (x3, y3)

def point_neg(P, mod):
    """Возвращает обратную точку (инверсию): -P"""
    if P == (None, None):
        return (None, None)
    x, y = P
    return (x, (-y) % mod)

def point_sub(P, Q, a_curve, mod):
    """Вычитание: P - Q = P + (-Q)"""
    return point_add(P, point_neg(Q, mod), a_curve, mod)

def scalar_mult(k, P, a_curve, mod):
    """Скалярное умножение точки P на число k: k*P"""
    if k == 0 or P == (None, None):
        return (None, None)

    result = (None, None)
    addend = P

    while k:
        if k & 1:
            result = point_add(result, addend, a_curve, mod)
        addend = point_add(addend, addend, a_curve, mod)
        k >>= 1
    return result

# ЗАДАНИЕ 1: Поиск точек и операции над точками

def find_points_on_curve(a_curve, b_curve, mod, x_range):
    """
    Находит все точки кривой для заданного диапазона x.
    Возвращает список точек (x, y).
    """
    points = []
    for x in x_range:
        y_sq = (pow(x, 3, mod) + a_curve * x + b_curve) % mod
        # Проверяем, является ли y_sq квадратичным вычетом по модулю mod
        for y in range(0, mod):
            if (y * y) % mod == y_sq:
                points.append((x, y))
                if y != 0 and y != mod - y:  # добавляем оба знака, но внутри цикла это учтётся
                    pass
        # Эффективный перебор (для простоты, для p=751 это может быть долговато,
        # но для диапазона 0-35 работает быстро)
    # Удалим возможные дубликаты
    return list(set(points))

# ЗАДАНИЕ 2: Шифрование / Расшифрование

# Отображение символов в точки на кривой (из таблицы 11.10 или аналогичной,
# но по условию для задания 2 используется своя таблица из табл. 11.9,
# в условии сказано, что могут быть приняты и иные таблицы.
# Для задания 2 берём полную таблицу 11.10, как более полную,
# так как в задании 2 даны только параметры, а таблица 11.9 не приведена в тексте полностью,
# но по контексту это таблица символов. Используем 11.10 для всех заданий, где нужно
# отображать символы в точки.

# Таблица 11.10 из теории
symbol_to_point = {
    'А': (189, 297), 'Б': (189, 454), 'В': (192, 32), 'Г': (192, 719),
    'Д': (194, 205), 'Е': (194, 546), 'Ж': (197, 145), 'З': (197, 606),
    'И': (198, 224), 'Й': (198, 527), 'К': (200, 30), 'Л': (200, 721),
    'М': (203, 324), 'Н': (203, 427), 'О': (205, 372), 'П': (205, 379),
    'Р': (206, 106), 'С': (206, 645), 'Т': (209, 82), 'У': (209, 669),
    'Ф': (210, 31), 'Х': (210, 720), 'Ц': (215, 247), 'Ч': (215, 504),
    'Ш': (218, 150), 'Щ': (218, 601), 'Ъ': (221, 138), 'Ы': (221, 613),
    'Ь': (226, 9), 'Э': (226, 742), 'Ю': (227, 299), 'Я': (227, 452)
}

# Обратное отображение для расшифрования
point_to_symbol = {v: k for k, v in symbol_to_point.items()}

def encrypt_message(message, G, Q, a_curve, mod):
    """Шифрование сообщения (строки) с использованием открытого ключа Q."""
    # Для каждого символа выбираем случайное k (для простоты и демонстрации используем фиксированное k=7,
    # но по заданию k - по собственному усмотрению)
    k = 7  # может быть выбрано произвольно
    encrypted = []
    for char in message.upper():
        if char not in symbol_to_point:
            raise ValueError(f"Символ '{char}' отсутствует в таблице")
        P = symbol_to_point[char]
        C1 = scalar_mult(k, G, a_curve, mod)
        C2 = point_add(P, scalar_mult(k, Q, a_curve, mod), a_curve, mod)
        encrypted.append((C1, C2))
    return encrypted

def decrypt_message(encrypted, d, a_curve, mod):
    """Расшифрование сообщения с использованием тайного ключа d."""
    decrypted = []
    for C1, C2 in encrypted:
        # P = C2 - d*C1
        dC1 = scalar_mult(d, C1, a_curve, mod)
        P = point_sub(C2, dC1, a_curve, mod)
        if P in point_to_symbol:
            decrypted.append(point_to_symbol[P])
        else:
            decrypted.append('?')
    return ''.join(decrypted)

# ЗАДАНИЕ 3: ЭЦП (ECDSA)

def ecdsa_sign(message, G, q, d, a_curve, mod):
    """
    Генерация подписи на основе алгоритма ECDSA.
    Возвращает (r, s), а также использованное k (для отладки/проверки)
    """
    # Определяем H(M) по первой букве фамилии
    H_M = None
    if message:
        first_char = message[0].upper()
        if first_char in symbol_to_point:
            x_first, _ = symbol_to_point[first_char]
            H_M = x_first % q
        else:
            raise ValueError(f"Символ '{first_char}' отсутствует в таблице")
    else:
        raise ValueError("Сообщение пустое")

    # Выбираем k (для демонстрации можно взять k=5, как в примере, или случайное)
    # По заданию k - по собственному усмотрению
    k = 5  # 1 < k < q
    while True:
        kG = scalar_mult(k, G, a_curve, mod)
        if kG == (None, None):
            k = (k % (q-1)) + 1
            continue
        x, _ = kG
        r = x % q
        if r == 0:
            k = (k % (q-1)) + 1
            continue
        # t = k^{-1} mod q
        try:
            t = inverse_mod(k, q)
        except ValueError:
            k = (k % (q-1)) + 1
            continue
        s = (t * (H_M + d * r)) % q
        if s == 0:
            k = (k % (q-1)) + 1
            continue
        break
    return (r, s), H_M, k

def ecdsa_verify(message, r, s, G, Q, q, a_curve, mod):
    """Верификация подписи (r, s) для сообщения."""
    if not (1 < r < q and 1 < s < q):
        return False, "r или s вне диапазона"

    # Вычисляем H(M)
    first_char = message[0].upper()
    if first_char not in symbol_to_point:
        return False, "Символ не найден"
    x_first, _ = symbol_to_point[first_char]
    H_M = x_first % q

    try:
        w = inverse_mod(s, q)
    except ValueError:
        return False, "Невозможно вычислить w"

    u1 = (w * H_M) % q
    u2 = (w * r) % q

    # Gu1 + Qu2
    P1 = scalar_mult(u1, G, a_curve, mod)
    P2 = scalar_mult(u2, Q, a_curve, mod)
    point = point_add(P1, P2, a_curve, mod)

    if point == (None, None):
        return False, "Получена точка O"
    x_prime, _ = point
    v = x_prime % q

    if v == r:
        return True, "Подпись верна"
    else:
        return False, "Подпись неверна"

# ОКОННОЕ ПРИЛОЖЕНИЕ (Tkinter)

class ECryptoApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Лабораторная работа №11 - Эллиптические кривые")

        # Создаем вкладки (Tabbed интерфейс)
        self.tab_control = tk.Frame(root)
        self.tab_control.pack(expand=1, fill='both')

        # Кнопки переключения
        self.btn_frame = tk.Frame(self.tab_control)
        self.btn_frame.pack(side=tk.TOP, fill=tk.X)

        self.btn_task1 = tk.Button(self.btn_frame, text="Задание 1", command=self.show_task1)
        self.btn_task1.pack(side=tk.LEFT, padx=2, pady=2)
        self.btn_task2 = tk.Button(self.btn_frame, text="Задание 2", command=self.show_task2)
        self.btn_task2.pack(side=tk.LEFT, padx=2, pady=2)
        self.btn_task3 = tk.Button(self.btn_frame, text="Задание 3", command=self.show_task3)
        self.btn_task3.pack(side=tk.LEFT, padx=2, pady=2)

        self.main_frame = tk.Frame(self.tab_control)
        self.main_frame.pack(expand=1, fill='both')

        self.current_task = None
        self.show_task1()

    def clear_frame(self):
        for widget in self.main_frame.winfo_children():
            widget.destroy()

    # --------------------------------------------------
    # ИНТЕРФЕЙС ЗАДАНИЯ 1
    # --------------------------------------------------
    def show_task1(self):
        self.clear_frame()
        self.current_task = 1

        label = tk.Label(self.main_frame, text="Задание 1: Операции над точками кривой\n"
                                               "ЭК: y^2 = x^3 - x + 1 (mod 751)\n"
                                               "Диапазон x: 481 - 515", font=("Arial", 12))
        label.pack(pady=10)

        # Кнопка поиска точек
        btn_find = tk.Button(self.main_frame, text="Найти точки в диапазоне x=481..515", command=self.task1_find_points)
        btn_find.pack(pady=5)

        # Текстовое поле для вывода точек
        self.task1_output = scrolledtext.ScrolledText(self.main_frame, width=70, height=10)
        self.task1_output.pack(pady=5)

        # Панель с операциями
        op_frame = tk.LabelFrame(self.main_frame, text="Выполнить операцию над точками P(58,139), Q(67,667), R(82,481)")
        op_frame.pack(pady=10, padx=10, fill=tk.X)

        btn_kP = tk.Button(op_frame, text="kP (k=8)", command=lambda: self.task1_operation('kP'))
        btn_kP.grid(row=0, column=0, padx=5, pady=5)

        btn_PQ = tk.Button(op_frame, text="P+Q", command=lambda: self.task1_operation('PQ'))
        btn_PQ.grid(row=0, column=1, padx=5, pady=5)

        btn_kPlQmR = tk.Button(op_frame, text="kP + lQ - R (k=8,l=11)", command=lambda: self.task1_operation('kPlQmR'))
        btn_kPlQmR.grid(row=0, column=2, padx=5, pady=5)

        btn_PmQR = tk.Button(op_frame, text="P - Q + R", command=lambda: self.task1_operation('PmQR'))
        btn_PmQR.grid(row=0, column=3, padx=5, pady=5)

        self.task1_result_label = tk.Label(self.main_frame, text="", font=("Arial", 10))
        self.task1_result_label.pack(pady=5)

    def task1_find_points(self):
        points = find_points_on_curve(a, b, p, range(481, 516))
        points.sort(key=lambda pt: (pt[0], pt[1]))
        self.task1_output.delete(1.0, tk.END)
        self.task1_output.insert(tk.END, f"Найдено {len(points)} точек:\n")
        for pt in points:
            self.task1_output.insert(tk.END, f"{pt}\n")

    def task1_operation(self, op_type):
        P = (48, 702)
        Q = (69, 241)
        R = (98, 338)
        k = 12
        l = 5

        try:
            if op_type == 'kP':
                res = scalar_mult(k, P, a, p)
                text = f"kP = {k} * {P} = {res}"
            elif op_type == 'PQ':
                res = point_add(P, Q, a, p)
                text = f"P + Q = {P} + {Q} = {res}"
            elif op_type == 'kPlQmR':
                term1 = scalar_mult(k, P, a, p)
                term2 = scalar_mult(l, Q, a, p)
                sum1 = point_add(term1, term2, a, p)
                res = point_sub(sum1, R, a, p)
                text = f"{k}P + {l}Q - R = {res}"
            elif op_type == 'PmQR':
                term1 = point_sub(P, Q, a, p)
                res = point_add(term1, R, a, p)
                text = f"P - Q + R = {res}"
            else:
                text = "Неизвестная операция"
            self.task1_result_label.config(text=text)
        except Exception as e:
            self.task1_result_label.config(text=f"Ошибка: {e}")

    # --------------------------------------------------
    # ИНТЕРФЕЙС ЗАДАНИЯ 2
    # --------------------------------------------------
    def show_task2(self):
        self.clear_frame()
        self.current_task = 2

        label = tk.Label(self.main_frame, text="Задание 2: Шифрование/расшифрование на основе ЭК\n"
                                               "G = (0,1), d = 34", font=("Arial", 12))
        label.pack(pady=10)

        # Ввод фамилии
        input_frame = tk.Frame(self.main_frame)
        input_frame.pack(pady=5)
        tk.Label(input_frame, text="Фамилия (или имя):").pack(side=tk.LEFT, padx=5)
        self.name_entry = tk.Entry(input_frame, width=30)
        self.name_entry.pack(side=tk.LEFT, padx=5)
        self.name_entry.insert(0, "Иванов")  # пример

        # Кнопка для вычисления открытого ключа Q
        self.q_label = tk.Label(self.main_frame, text="Q = d*G = не вычислено", font=("Arial", 10))
        self.q_label.pack(pady=5)

        btn_calc_Q = tk.Button(self.main_frame, text="Вычислить открытый ключ Q", command=self.task2_calc_Q)
        btn_calc_Q.pack(pady=5)

        # Кнопки шифрования/расшифрования
        btn_encrypt = tk.Button(self.main_frame, text="Зашифровать", command=self.task2_encrypt)
        btn_encrypt.pack(pady=5)

        btn_decrypt = tk.Button(self.main_frame, text="Расшифровать (используется последний шифртекст)", command=self.task2_decrypt)
        btn_decrypt.pack(pady=5)

        self.task2_output = scrolledtext.ScrolledText(self.main_frame, width=70, height=8)
        self.task2_output.pack(pady=5)

        # Хранилище для последнего шифртекста и Q
        self.last_encrypted = None
        self.Q_task2 = None

    def task2_calc_Q(self):
        G = (0, 1)
        d = 34
        self.Q_task2 = scalar_mult(d, G, a, p)
        self.q_label.config(text=f"Q = d*G = {d} * {G} = {self.Q_task2}")

    def task2_encrypt(self):
        if self.Q_task2 is None:
            messagebox.showerror("Ошибка", "Сначала вычислите открытый ключ Q")
            return
        name = self.name_entry.get().strip().upper()
        if not name:
            messagebox.showerror("Ошибка", "Введите фамилию/имя")
            return
        G = (0, 1)
        try:
            encrypted = encrypt_message(name, G, self.Q_task2, a, p)
            self.last_encrypted = encrypted
            result = "Шифртекст (C1, C2):\n"
            for i, (c1, c2) in enumerate(encrypted):
                result += f"{name[i]}: C1={c1}, C2={c2}\n"
            self.task2_output.delete(1.0, tk.END)
            self.task2_output.insert(tk.END, result)
        except Exception as e:
            messagebox.showerror("Ошибка", str(e))

    def task2_decrypt(self):
        if self.last_encrypted is None:
            messagebox.showerror("Ошибка", "Нет зашифрованного сообщения")
            return
        if self.Q_task2 is None:
            messagebox.showerror("Ошибка", "Открытый ключ не вычислен")
            return
        d = 41
        try:
            decrypted = decrypt_message(self.last_encrypted, d, a, p)
            self.task2_output.insert(tk.END, f"\nРасшифрованное сообщение: {decrypted}\n")
        except Exception as e:
            messagebox.showerror("Ошибка", str(e))

    # --------------------------------------------------
    # ИНТЕРФЕЙС ЗАДАНИЯ 3
    # --------------------------------------------------
    def show_task3(self):
        self.clear_frame()
        self.current_task = 3

        label = tk.Label(self.main_frame, text="Задание 3: ЭЦП (ECDSA)\n"
                                               "G = (416,55), q = 13, d = 3 (ваш вариант)", font=("Arial", 12))
        label.pack(pady=10)

        # Ввод сообщения (фамилии) для подписи
        input_frame = tk.Frame(self.main_frame)
        input_frame.pack(pady=5)
        tk.Label(input_frame, text="Сообщение (фамилия):").pack(side=tk.LEFT, padx=5)
        self.msg_entry = tk.Entry(input_frame, width=30)
        self.msg_entry.pack(side=tk.LEFT, padx=5)
        self.msg_entry.insert(0, "Петров")

        # Вычисление Q
        self.q3_label = tk.Label(self.main_frame, text="Q = d*G = не вычислено", font=("Arial", 10))
        self.q3_label.pack(pady=5)

        btn_calc_Q3 = tk.Button(self.main_frame, text="Вычислить открытый ключ Q", command=self.task3_calc_Q)
        btn_calc_Q3.pack(pady=5)

        # Подпись и верификация
        btn_sign = tk.Button(self.main_frame, text="Сгенерировать подпись", command=self.task3_sign)
        btn_sign.pack(pady=5)

        btn_verify = tk.Button(self.main_frame, text="Верифицировать подпись", command=self.task3_verify)
        btn_verify.pack(pady=5)

        self.task3_output = scrolledtext.ScrolledText(self.main_frame, width=70, height=8)
        self.task3_output.pack(pady=5)

        self.signature = None
        self.H_M_used = None
        self.Q_task3 = None
        self.k_used = None

    def task3_calc_Q(self):
        G = (416, 55)
        d = 9
        self.Q_task3 = scalar_mult(d, G, a, p)
        self.q3_label.config(text=f"Q = d*G = {d} * {G} = {self.Q_task3}")

    def task3_sign(self):
        if self.Q_task3 is None:
            messagebox.showerror("Ошибка", "Сначала вычислите открытый ключ Q")
            return
        message = self.msg_entry.get().strip()
        if not message:
            messagebox.showerror("Ошибка", "Введите сообщение")
            return
        G = (416, 55)
        q = 13
        d = 9
        try:
            (r, s), H_M, k = ecdsa_sign(message, G, q, d, a, p)
            self.signature = (r, s)
            self.H_M_used = H_M
            self.k_used = k
            res = f"Сообщение: {message}\nH(M) = {H_M}\nПодпись: r = {r}, s = {s}"
            self.task3_output.delete(1.0, tk.END)
            self.task3_output.insert(tk.END, res)
        except Exception as e:
            messagebox.showerror("Ошибка", str(e))

    def task3_verify(self):
        if self.signature is None:
            messagebox.showerror("Ошибка", "Сначала сгенерируйте подпись")
            return
        if self.Q_task3 is None:
            messagebox.showerror("Ошибка", "Открытый ключ не вычислен")
            return
        message = self.msg_entry.get().strip()
        G = (416, 55)
        q = 13
        r, s = self.signature
        try:
            result, msg = ecdsa_verify(message, r, s, G, self.Q_task3, q, a, p)
            if result:
                out = f"Подпись верна! (v = r)"
            else:
                out = f"Подпись НЕ верна: {msg}"
            self.task3_output.insert(tk.END, f"\nВерификация: {out}")
        except Exception as e:
            messagebox.showerror("Ошибка", str(e))

if __name__ == "__main__":
    root = tk.Tk()
    app = ECryptoApp(root)
    root.mainloop()