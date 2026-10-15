import tkinter as tk
import time

# KSA с подробным выводом первых 3 шагов
def ksa_verbose(key, n, log):
    S = list(range(2**n))
    j = 0

    log.insert(tk.END, f"Начальный S (0..{len(S)-1}):\n{S}\n\n")

    for i in range(len(S)):
        old_j = j
        j = (j + S[i] + key[i % len(key)]) % len(S)

        if i < 3:
            log.insert(tk.END, f"=== KSA шаг {i} ===\n")

            log.insert(tk.END, "Формула:\n")
            log.insert(tk.END, "j = (j + S[i] + key[i mod len(key)]) mod N\n\n")

            log.insert(tk.END, "Подстановка:\n")
            log.insert(tk.END,
                f"j = ({old_j} + {S[i]} + {key[i % len(key)]}) mod {len(S)}\n"
            )

            log.insert(tk.END, f"j = {j}\n\n")

            log.insert(tk.END,
                f"Обмен:\nS[{i}] = {S[i]} ↔ S[{j}] = {S[j]}\n"
            )

            S[i], S[j] = S[j], S[i]

            log.insert(tk.END, f"S после обмена:\n{S}\n\n")

        else:
            S[i], S[j] = S[j], S[i]

    log.insert(tk.END, "...\n(остальные шаги KSA выполнены без вывода)\n\n")
    
    log.insert(tk.END, "=== Итоговый массив S после KSA ===\n")
    log.insert(tk.END, f"{S}\n\n")

    return S


# PRGA (можно оставить подробным)
def prga_verbose(S, length, log):
    i = 0
    j = 0
    keystream = []

    N = len(S)

    log.insert(tk.END, "=== PRGA ===\n\n")

    for step in range(length):
        log.insert(tk.END, f"=== Шаг {step} ===\n")

        # i
        old_i = i
        i = (i + 1) % N

        log.insert(tk.END, "Формула для i:\n")
        log.insert(tk.END, "i = (i + 1) mod N\n")
        log.insert(tk.END, f"i = ({old_i} + 1) mod {N} = {i}\n\n")

        # j
        old_j = j
        j = (j + S[i]) % N

        log.insert(tk.END, "Формула для j:\n")
        log.insert(tk.END, "j = (j + S[i]) mod N\n")
        log.insert(tk.END, f"j = ({old_j} + {S[i]}) mod {N} = {j}\n\n")

        # swap
        log.insert(tk.END,
            f"Обмен:\nS[{i}] = {S[i]} ↔ S[{j}] = {S[j]}\n"
        )

        S[i], S[j] = S[j], S[i]

        log.insert(tk.END, f"S после обмена:\n{S}\n\n")

        # K
        t = (S[i] + S[j]) % N
        K = S[t]
        keystream.append(K)

        log.insert(tk.END, "Формула для K:\n")
        log.insert(tk.END, "K = S[(S[i] + S[j]) mod N]\n")
        log.insert(tk.END,
            f"K = S[({S[i]} + {S[j]}) mod {N}] = S[{t}] = {K}\n\n"
        )

    return keystream

# RC4
def rc4_verbose(key, text, n, log):
    S = ksa_verbose(key, n, log)
    ks = prga_verbose(S, len(text), log)

    result = []

    log.insert(tk.END, "=== XOR (шифрование) ===\n\n")

    for i, c in enumerate(text):
        val = ord(c)
        enc = val ^ ks[i]

        log.insert(tk.END,
            f"{c} ({val}) XOR {ks[i]} = {enc}\n"
        )

        result.append(enc)

    return result


# запуск
def run_rc4():
    output.delete(1.0, tk.END)

    key = [20, 21, 22, 23, 60, 61]
    text = input_text.get()

    if not text:
        output.insert(tk.END, "Введите текст!\n")
        return

    output.insert(tk.END, f"Ключ: {key}\n")
    output.insert(tk.END, f"n = 6 → размер S = {2**6} элементов\n")
    output.insert(tk.END, f"Текст: {text}\n\n")

    start = time.time()
    result = rc4_verbose(key, text, 6, output)
    end = time.time()

    output.insert(tk.END, "\n=== Результат (в виде чисел) ===\n")
    output.insert(tk.END, str(result) + "\n")

    output.insert(tk.END, "\n=== Время выполнения ===\n")
    output.insert(tk.END, f"{end - start:.6f} сек\n")


# GUI
root = tk.Tk()
root.title("RC4 (подробно, с формулами KSA)")

tk.Label(root, text="Введите текст:").pack()

input_text = tk.Entry(root, width=50)
input_text.pack()

btn = tk.Button(root, text="Запустить", command=run_rc4)
btn.pack()

output = tk.Text(root, height=35, width=100)
output.pack()

root.mainloop()