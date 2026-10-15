import math
import time
import matplotlib.pyplot as plt
from collections import Counter


# =============================
# ЧТЕНИЕ ТЕКСТА
# =============================

def read_text(filename):
    with open(filename, "r", encoding="utf-8") as f:
        text = f.read().lower()

    text = text.replace("\n", " ")
    return text


# =============================
# ПЕЧАТЬ ТАБЛИЦЫ
# =============================

def print_table(table):
    for row in table:
        print(" ".join(row))


# =============================
# МАРШРУТНАЯ ПЕРЕСТАНОВКА
# =============================

def route_encrypt(text, rows):

    cols = math.ceil(len(text) / rows)

    table = [[" " for _ in range(cols)] for _ in range(rows)]

    index = 0

    for i in range(rows):
        for j in range(cols):
            if index < len(text):
                table[i][j] = text[index]
                index += 1

    print("\nТаблица маршрутной перестановки:")
    print_table(table)

    cipher = ""

    for col in range(cols):

        if col % 2 == 0:   # вниз
            for row in range(rows):
                cipher += table[row][col]

        else:              # вверх
            for row in reversed(range(rows)):
                cipher += table[row][col]

    return cipher


def route_decrypt(cipher, rows):

    cols = math.ceil(len(cipher) / rows)

    table = [["" for _ in range(cols)] for _ in range(rows)]

    index = 0

    for col in range(cols):

        if col % 2 == 0:
            for row in range(rows):
                if index < len(cipher):
                    table[row][col] = cipher[index]
                    index += 1

        else:
            for row in reversed(range(rows)):
                if index < len(cipher):
                    table[row][col] = cipher[index]
                    index += 1

    text = ""

    for i in range(rows):
        for j in range(cols):
            text += table[i][j]

    return text.strip()


# =============================
# МНОЖЕСТВЕННАЯ ПЕРЕСТАНОВКА
# =============================

def columnar_encrypt(text, key):

    key_len = len(key)

    rows = math.ceil(len(text) / key_len)

    table = [[" " for _ in range(key_len)] for _ in range(rows)]

    index = 0

    for i in range(rows):
        for j in range(key_len):
            if index < len(text):
                table[i][j] = text[index]
                index += 1

    order = sorted(range(len(key)), key=lambda k: key[k])

    cipher = ""

    for col in order:
        for row in table:
            cipher += row[col]

    return cipher


def columnar_decrypt(cipher, key):

    key_len = len(key)

    rows = math.ceil(len(cipher) / key_len)

    order = sorted(range(len(key)), key=lambda k: key[k])

    table = [["" for _ in range(key_len)] for _ in range(rows)]

    index = 0

    for col in order:
        for row in range(rows):
            if index < len(cipher):
                table[row][col] = cipher[index]
                index += 1

    text = ""

    for row in table:
        text += "".join(row)

    return text.strip()


# =============================
# ГИСТОГРАММА
# =============================

def plot_histogram(text, title):

    freq = Counter(text)

    plt.figure()

    plt.bar(freq.keys(), freq.values())

    plt.title(title)

    plt.xlabel("Symbol")

    plt.ylabel("Frequency")

    plt.show()


# =============================
# ОСНОВНАЯ ПРОГРАММА
# =============================

text = read_text("text.txt")

print("\n=============================")
print("ИСХОДНЫЙ ТЕКСТ")
print("=============================")

print(text)
print("\nДлина текста:", len(text))


# =============================
# МАРШРУТНАЯ ПЕРЕСТАНОВКА
# =============================

rows = 5

start = time.time()

route_cipher = route_encrypt(text, rows)

encrypt_time = time.time() - start


start = time.time()

route_plain = route_decrypt(route_cipher, rows)

decrypt_time = time.time() - start


print("\n=============================")
print("МАРШРУТНАЯ ПЕРЕСТАНОВКА")
print("=============================")

print("\nЗашифрованный текст:")
print(route_cipher)

print("\nРасшифрованный текст:")
print(route_plain)

print("\nВремя шифрования:", round(encrypt_time * 1000, 3), "ms")
print("Время расшифрования:", round(decrypt_time * 1000, 3), "ms")


# =============================
# МНОЖЕСТВЕННАЯ ПЕРЕСТАНОВКА
# =============================

key = "varvarakazakova"

start = time.time()

multi_cipher = columnar_encrypt(text, key)

encrypt_time2 = time.time() - start


start = time.time()

multi_plain = columnar_decrypt(multi_cipher, key)

decrypt_time2 = time.time() - start


print("\n=============================")
print("МНОЖЕСТВЕННАЯ ПЕРЕСТАНОВКА")
print("=============================")

print("\nЗашифрованный текст:")
print(multi_cipher)

print("\nРасшифрованный текст:")
print(multi_plain)

print("\nВремя шифрования:", round(encrypt_time2 * 1000, 3), "ms")
print("Время расшифрования:", round(decrypt_time2 * 1000, 3), "ms")


# =============================
# ГИСТОГРАММЫ
# =============================

plot_histogram(text, "Original text frequency")

plot_histogram(route_cipher, "Route cipher frequency")

plot_histogram(multi_cipher, "Multiple permutation frequency")