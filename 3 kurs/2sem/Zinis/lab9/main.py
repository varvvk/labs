import tkinter as tk
from tkinter import scrolledtext
import hashlib
import time
import random
import string


def hash_text():
    text = input_text.get("1.0", tk.END).strip()

    if not text:
        result_label.config(text="Введите текст!")
        return

    start = time.perf_counter()

    hash_object = hashlib.sha256(text.encode())
    hash_result = hash_object.hexdigest()

    end = time.perf_counter()

    result_label.config(text=f"Хеш:\n{hash_result}")
    time_label.config(text=f"Время: {end - start:.8f} сек")


def benchmark():
    sizes = [10, 100, 1000, 10000, 100000]
    results = ""

    for size in sizes:
        test_data = ''.join(random.choices(string.ascii_letters, k=size))

        iterations = 1000
        start = time.perf_counter()

        for _ in range(iterations):
            hashlib.sha256(test_data.encode()).hexdigest()

        end = time.perf_counter()

        avg_time = (end - start) / iterations
        results += f"{size} символов, {avg_time:.10f} сек\n"

    benchmark_text.delete("1.0", tk.END)
    benchmark_text.insert(tk.END, results)


root = tk.Tk()
root.title("SHA-256 Hash App")
root.geometry("600x500")

tk.Label(root, text="Введите текст:").pack()
input_text = scrolledtext.ScrolledText(root, height=5)
input_text.pack(fill="both", padx=10, pady=5)

tk.Button(root, text="Вычислить хеш", command=hash_text).pack(pady=5)

result_label = tk.Label(root, text="", wraplength=550, justify="left")
result_label.pack(pady=5)

time_label = tk.Label(root, text="")
time_label.pack()

tk.Button(root, text="Тест скорости", command=benchmark).pack(pady=5)

benchmark_text = scrolledtext.ScrolledText(root, height=10)
benchmark_text.pack(fill="both", padx=10, pady=5)

root.mainloop()