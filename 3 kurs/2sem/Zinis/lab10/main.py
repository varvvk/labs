import tkinter as tk
from tkinter import ttk, scrolledtext
import hashlib
import random
import time
from math import gcd

def hash_message(message):
    return int(hashlib.sha256(message.encode()).hexdigest(), 16)

def mod_inverse(a, m):
    return pow(a, -1, m)

def is_prime(n):
    if n < 2:
        return False

    for i in range(2, int(n ** 0.5) + 1):
        if n % i == 0:
            return False

    return True

def generate_prime(start=100, end=500):
    while True:
        p = random.randint(start, end)

        if is_prime(p):
            return p

class RSA:

    @staticmethod
    def generate_keys():
        p = generate_prime()
        q = generate_prime()

        while p == q:
            q = generate_prime()

        n = p * q
        phi = (p - 1) * (q - 1)

        e = 65537

        while gcd(e, phi) != 1:
            e += 2

        d = mod_inverse(e, phi)

        return (e, n), (d, n)

    @staticmethod
    def sign(message, private_key):
        d, n = private_key

        h = hash_message(message)

        signature = pow(h, d, n)

        return signature

class ElGamal:

    @staticmethod
    def generate_keys():
        p = generate_prime(300, 700)

        g = random.randint(2, p - 2)

        x = random.randint(2, p - 2)

        y = pow(g, x, p)

        public_key = (p, g, y)

        private_key = x

        return public_key, private_key

    @staticmethod
    def sign(message, public_key, private_key):
        p, g, y = public_key

        x = private_key

        h = hash_message(message)

        while True:
            k = random.randint(2, p - 2)

            if gcd(k, p - 1) == 1:
                break

        a = pow(g, k, p)

        k_inv = mod_inverse(k, p - 1)

        b = ((h - x * a) * k_inv) % (p - 1)

        return (a, b)

class Schnorr:

    @staticmethod
    def generate_keys():
        p = generate_prime(300, 700)

        q = generate_prime(50, 150)

        g = random.randint(2, p - 2)

        x = random.randint(1, q - 1)

        y = mod_inverse(pow(g, x, p), p)

        public_key = (p, q, g, y)

        private_key = x

        return public_key, private_key

    @staticmethod
    def sign(message, public_key, private_key):
        p, q, g, y = public_key

        x = private_key

        k = random.randint(1, q - 1)

        a = pow(g, k, p)

        h = hash_message(message + str(a)) % q

        b = (k + x * h) % q

        return (h, b)

class DigitalSignatureApp:

    def __init__(self, root):
        self.root = root

        self.root.title("Электронная цифровая подпись")

        self.root.geometry("900x650")

        self.create_widgets()

    def create_widgets(self):

        title = tk.Label(
            self.root,
            text="Электронная цифровая подпись",
            font=("Arial", 18, "bold")
        )

        title.pack(pady=10)

        message_label = tk.Label(
            self.root,
            text="Введите сообщение:",
            font=("Arial", 12)
        )

        message_label.pack()

        self.message_entry = tk.Entry(
            self.root,
            width=80,
            font=("Arial", 12)
        )

        self.message_entry.pack(pady=5)

        self.message_entry.insert(
            0,
            "Электронная цифровая подпись"
        )

        # Выбор алгоритма
        algo_label = tk.Label(
            self.root,
            text="Выберите алгоритм:",
            font=("Arial", 12)
        )

        algo_label.pack(pady=5)

        self.algorithm = ttk.Combobox(
            self.root,
            values=["RSA", "Эль-Гамаль", "Шнорр"],
            state="readonly",
            width=30,
            font=("Arial", 12)
        )

        self.algorithm.current(0)

        self.algorithm.pack(pady=5)

        generate_button = tk.Button(
            self.root,
            text="Сгенерировать ЭЦП",
            font=("Arial", 12, "bold"),
            width=25,
            command=self.generate_signature
        )

        generate_button.pack(pady=10)

        # Поле вывода
        self.output = scrolledtext.ScrolledText(
            self.root,
            width=100,
            height=25,
            font=("Consolas", 11)
        )

        self.output.pack(pady=10)

    def generate_signature(self):

        self.output.delete(1.0, tk.END)

        message = self.message_entry.get()

        algorithm = self.algorithm.get()

        start_time = time.perf_counter()

        # RSA
        if algorithm == "RSA":

            public_key, private_key = RSA.generate_keys()

            signature = RSA.sign(
                message,
                private_key
            )

            end_time = time.perf_counter()

            self.output.insert(
                tk.END,
                "АЛГОРИТМ RSA\n\n"
            )

            self.output.insert(
                tk.END,
                f"Открытый ключ:\n{public_key}\n\n"
            )

            self.output.insert(
                tk.END,
                f"Закрытый ключ:\n{private_key}\n\n"
            )

            self.output.insert(
                tk.END,
                f"Подпись:\n{signature}\n\n"
            )

        # Эль-Гамаль
        elif algorithm == "Эль-Гамаль":

            public_key, private_key = ElGamal.generate_keys()

            signature = ElGamal.sign(
                message,
                public_key,
                private_key
            )

            end_time = time.perf_counter()

            self.output.insert(
                tk.END,
                "АЛГОРИТМ ЭЛЬ-ГАМАЛЯ\n\n"
            )

            self.output.insert(
                tk.END,
                f"Открытый ключ:\n{public_key}\n\n"
            )

            self.output.insert(
                tk.END,
                f"Закрытый ключ:\n{private_key}\n\n"
            )

            self.output.insert(
                tk.END,
                f"Подпись:\n{signature}\n\n"
            )

        # Шнорр
        elif algorithm == "Шнорр":

            public_key, private_key = Schnorr.generate_keys()

            signature = Schnorr.sign(
                message,
                public_key,
                private_key
            )

            end_time = time.perf_counter()

            self.output.insert(
                tk.END,
                "АЛГОРИТМ ШНОРРА\n\n"
            )

            self.output.insert(
                tk.END,
                f"Открытый ключ:\n{public_key}\n\n"
            )

            self.output.insert(
                tk.END,
                f"Закрытый ключ:\n{private_key}\n\n"
            )

            self.output.insert(
                tk.END,
                f"Подпись:\n{signature}\n\n"
            )

        generation_time = end_time - start_time

        self.output.insert(
            tk.END,
            f"Время генерации ЭЦП: "
            f"{generation_time:.6f} сек\n"
        )

root = tk.Tk()

app = DigitalSignatureApp(root)

root.mainloop()