import tkinter as tk
from random import getrandbits
from sympy import nextprime

# генерация простого числа
def generate_prime(bits=256):
    base = getrandbits(bits)
    prime = nextprime(base)
    return base, prime

# RSA ПСП (подробно)
def rsa_prsg_verbose(p, q, e, seed, length, log):
    n = p * q
    log.insert(tk.END, f"n = p * q = {n}\n\n")

    x = seed
    bits = []

    log.insert(tk.END, f"Начальное значение x0 = {x}\n\n")

    for i in range(length):
        log.insert(tk.END, f"Шаг {i+1}:\n")

        x = pow(x, e, n)
        bit = x % 2
        bits.append(bit)

        log.insert(tk.END, f"x{i+1} = {x}\n")
        log.insert(tk.END, f"b{i+1} = {bit}\n\n")

    return bits

def run_rsa():
    output.delete(1.0, tk.END)

    # параметры
    base_p, p = generate_prime()
    base_q, q = generate_prime()
    e = 65537
    seed = 123456

    try:
        length = int(length_entry.get())
    except:
        length = 10

    # вывод генерации p и q
    output.insert(tk.END, "=== Генерация p ===\n")
    output.insert(tk.END, f"Простое p: {p}\n\n")

    output.insert(tk.END, "=== Генерация q ===\n")
    output.insert(tk.END, f"Простое q: {q}\n\n")

    # RSA параметры
    n = p * q
    output.insert(tk.END, "=== Параметры RSA ===\n")
    output.insert(tk.END, f"n = {n}\n")
    output.insert(tk.END, f"e = {e}\n")
    output.insert(tk.END, f"x0 = {seed}\n\n")

    # генерация ПСП
    bits = rsa_prsg_verbose(p, q, e, seed, length, output)

    # итог
    output.insert(tk.END, "=== Итоговая битовая последовательность ===\n")
    bit_string = "".join(map(str, bits))
    output.insert(tk.END, bit_string + "\n")


# GUI
root = tk.Tk()
root.title("RSA ПСП (подробно)")

frame = tk.Frame(root)
frame.pack()

tk.Label(frame, text="Длина ПСП:").pack(side=tk.LEFT)
length_entry = tk.Entry(frame, width=10)
length_entry.insert(0, "10")
length_entry.pack(side=tk.LEFT)

btn = tk.Button(root, text="Сгенерировать", command=run_rsa)
btn.pack()

output = tk.Text(root, height=35, width=100)
output.pack()

root.mainloop()