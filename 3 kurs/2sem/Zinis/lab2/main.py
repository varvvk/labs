import string
import time
from collections import Counter
import matplotlib.pyplot as plt

ALPHABET = string.ascii_uppercase


# =========================
# ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ
# =========================

def prepare_text(text):
    """Оставляем только английские буквы и приводим к верхнему регистру"""
    return ''.join([c for c in text.upper() if c in ALPHABET])


def create_keyword_alphabet(keyword):
    """Создание алфавита с ключевым словом"""
    keyword = prepare_text(keyword)
    seen = set()
    result = ""

    for char in keyword + ALPHABET:
        if char not in seen:
            seen.add(char)
            result += char

    return result


# =========================
# 1️⃣ ШИФР ЦЕЗАРЯ С КЛЮЧЕВЫМ СЛОВОМ
# =========================

def caesar_encrypt(text, keyword, shift):
    keyword_alphabet = create_keyword_alphabet(keyword)
    shifted = keyword_alphabet[shift:] + keyword_alphabet[:shift]

    result = ""

    for char in text:
        if char in ALPHABET:
            index = keyword_alphabet.index(char)
            result += shifted[index]
        else:
            result += char

    return result


def caesar_decrypt(text, keyword, shift):
    keyword_alphabet = create_keyword_alphabet(keyword)
    shifted = keyword_alphabet[shift:] + keyword_alphabet[:shift]

    result = ""

    for char in text:
        if char in ALPHABET:
            index = shifted.index(char)
            result += keyword_alphabet[index]
        else:
            result += char

    return result


# =========================
# 2️⃣ ТАБЛИЦА ТРИСЕМУСА
# =========================

def create_trisemus_table(keyword):
    key_alphabet = create_keyword_alphabet(keyword)

    table = []
    index = 0

    for i in range(5):
        row = []
        for j in range(5):
            if index < 25:
                row.append(key_alphabet[index])
                index += 1
        table.append(row)

    return table


def find_position(table, char):
    for i in range(5):
        for j in range(5):
            if table[i][j] == char:
                return i, j
    return None


def trisemus_encrypt(text, keyword):
    table = create_trisemus_table(keyword)
    result = ""

    for char in text:
        if char in ALPHABET and char != 'J':  # таблица 5x5
            row, col = find_position(table, char)
            new_row = (row + 1) % 5
            result += table[new_row][col]
        else:
            result += char

    return result


def trisemus_decrypt(text, keyword):
    table = create_trisemus_table(keyword)
    result = ""

    for char in text:
        if char in ALPHABET and char != 'J':
            row, col = find_position(table, char)
            new_row = (row - 1) % 5
            result += table[new_row][col]
        else:
            result += char

    return result


# =========================
# 📊 ГИСТОГРАММА ЧАСТОТ
# =========================

def plot_histogram(text, title, filename):
    freq = Counter(text)

    letters = sorted(freq.keys())
    values = [freq[letter] for letter in letters]

    plt.figure()
    plt.bar(letters, values)
    plt.title(title)
    plt.xlabel("Symbols")
    plt.ylabel("Frequency")
    plt.savefig(filename)
    plt.show()


# =========================
# 📂 РАБОТА С ФАЙЛАМИ
# =========================

def process_file(input_file, output_file, method, mode):
    with open(input_file, "r", encoding="utf-8") as f:
        original_text = f.read()

    if len(original_text) < 5000:
        print("❗ Внимание: объем текста менее 5000 символов")

    text = prepare_text(original_text)

    start_time = time.perf_counter()

    if method == "1":
        keyword = "Kazakova"
        shift = 24
        if mode == "e":
            result = caesar_encrypt(text, keyword, shift)
        else:
            result = caesar_decrypt(text, keyword, shift)

    elif method == "2":
        keyword = "Varvara"
        if mode == "e":
            result = trisemus_encrypt(text, keyword)
        else:
            result = trisemus_decrypt(text, keyword)

    else:
        print("Неверный метод")
        return

    end_time = time.perf_counter()
    execution_time = end_time - start_time

    print(f"⏱ Время выполнения: {execution_time:.6f} секунд")

    with open(output_file, "w", encoding="utf-8") as f:
        f.write(result)

    print("✅ Готово! Результат сохранен в", output_file)

    # Построение гистограмм
    plot_histogram(text, "Histogram of Original Text", "original_histogram.png")
    plot_histogram(result, "Histogram of Encrypted Text", "encrypted_histogram.png")


# =========================
# 🚀 МЕНЮ
# =========================

def main():
    print("1 - Шифр Цезаря (Kazakova, a=24)")
    print("2 - Таблица Трисемуса (Varvara)")

    method = input("Выберите метод: ")
    mode = input("e - шифрование, d - расшифрование: ")

    input_file = input("Введите имя входного файла: ")
    output_file = input("Введите имя выходного файла: ")

    process_file(input_file, output_file, method, mode)


if __name__ == "__main__":
    main()