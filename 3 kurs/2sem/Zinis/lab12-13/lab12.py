import tkinter as tk
from tkinter import filedialog, messagebox, ttk
from PIL import Image, ImageTk
import numpy as np
import os


class LSBSteganographyApp:
    def __init__(self, root):
        self.root = root
        self.root.title("LSB Steganography")
        self.root.geometry("1200x750")
        self.root.configure(bg="white")

        self.image_path = None
        self.image = None
        self.stego_image = None

        self.create_ui()

    def create_ui(self):
        top_frame = tk.Frame(self.root, bg="white")
        top_frame.pack(pady=10)

        tk.Button(
            top_frame,
            text="Выбрать изображение",
            command=self.load_image,
            bg="black",
            fg="white",
            width=25,
            height=2
        ).grid(row=0, column=0, padx=5)

        self.method_var = tk.StringVar(value="Последовательно")

        methods = ttk.Combobox(
            top_frame,
            textvariable=self.method_var,
            values=["Последовательно", "Шахматный порядок"],
            state="readonly",
            width=25
        )
        methods.grid(row=0, column=1, padx=5)

        self.message_type = tk.StringVar(value="fio")

        tk.Radiobutton(
            top_frame,
            text="ФИО",
            variable=self.message_type,
            value="fio",
            bg="white"
        ).grid(row=0, column=2, padx=5)

        tk.Radiobutton(
            top_frame,
            text="Текст лабораторной",
            variable=self.message_type,
            value="lab",
            bg="white"
        ).grid(row=0, column=3, padx=5)

        self.text_input = tk.Text(
            self.root,
            height=8,
            width=100,
            font=("Consolas", 11),
            bg="white",
            fg="black",
            insertbackground="black"
        )
        self.text_input.pack(pady=10)

        button_frame = tk.Frame(self.root, bg="white")
        button_frame.pack()

        tk.Button(
            button_frame,
            text="Скрыть сообщение",
            command=self.hide_message,
            bg="black",
            fg="white",
            width=25,
            height=2
        ).grid(row=0, column=0, padx=10)

        tk.Button(
            button_frame,
            text="Извлечь сообщение",
            command=self.extract_message,
            bg="black",
            fg="white",
            width=25,
            height=2
        ).grid(row=0, column=1, padx=10)

        tk.Button(
            button_frame,
            text="Показать LSB матрицы",
            command=self.show_lsb_planes,
            bg="black",
            fg="white",
            width=25,
            height=2
        ).grid(row=0, column=2, padx=10)

        image_frame = tk.Frame(self.root, bg="white")
        image_frame.pack(pady=15)

        self.original_label = tk.Label(image_frame, bg="white")
        self.original_label.grid(row=0, column=0, padx=20)

        self.stego_label = tk.Label(image_frame, bg="white")
        self.stego_label.grid(row=0, column=1, padx=20)

    def load_image(self):
        path = filedialog.askopenfilename(
            filetypes=[("Image Files", "*.png *.bmp *.jpg")]
        )

        if not path:
            return

        self.image_path = path
        self.image = Image.open(path).convert("RGB")

        self.display_image(self.image, self.original_label)

    def display_image(self, image, label):
        preview = image.copy()
        preview.thumbnail((400, 400))

        photo = ImageTk.PhotoImage(preview)
        label.config(image=photo)
        label.image = photo

    def text_to_bits(self, text):
        binary = ''.join(format(ord(char), '08b') for char in text)
        binary += '1111111111111110'
        return binary

    def bits_to_text(self, bits):
        chars = []

        for i in range(0, len(bits), 8):
            byte = bits[i:i + 8]

            if byte == '11111110':
                break

            chars.append(chr(int(byte, 2)))

        return ''.join(chars)

    def hide_message(self):
        if self.image is None:
            messagebox.showerror("Ошибка", "Загрузите изображение")
            return

        message = self.text_input.get("1.0", tk.END).strip()

        if not message:
            messagebox.showerror("Ошибка", "Введите текст")
            return

        binary = self.text_to_bits(message)

        img_array = np.array(self.image)
        flat = img_array.flatten()

        if len(binary) > len(flat):
            messagebox.showerror("Ошибка", "Сообщение слишком большое")
            return

        method = self.method_var.get()

        if method == "Последовательно":
            indexes = range(len(binary))
        else:
            indexes = []
            even = list(range(0, len(flat), 2))
            odd = list(range(1, len(flat), 2))
            indexes = even + odd

        for i, bit in enumerate(binary):
            idx = indexes[i]
            flat[idx] = (flat[idx] & 254) | int(bit)

        stego_array = flat.reshape(img_array.shape)

        self.stego_image = Image.fromarray(stego_array.astype(np.uint8))

        self.display_image(self.stego_image, self.stego_label)

        save_path = filedialog.asksaveasfilename(
            defaultextension=".png",
            filetypes=[("PNG", "*.png")]
        )

        if save_path:
            self.stego_image.save(save_path)
            messagebox.showinfo("Успех", "Сообщение скрыто")

    def extract_message(self):
        path = filedialog.askopenfilename(
            filetypes=[("Image Files", "*.png *.bmp *.jpg")]
        )

        if not path:
            return

        image = Image.open(path).convert("RGB")
        img_array = np.array(image)
        flat = img_array.flatten()

        method = self.method_var.get()

        if method == "Последовательно":
            indexes = range(len(flat))
        else:
            even = list(range(0, len(flat), 2))
            odd = list(range(1, len(flat), 2))
            indexes = even + odd

        bits = ""

        for idx in indexes:
            bits += str(flat[idx] & 1)

            if bits.endswith('11111110'):
                break

        text = self.bits_to_text(bits)

        self.text_input.delete("1.0", tk.END)
        self.text_input.insert(tk.END, text)

        messagebox.showinfo("Извлечение", "Сообщение извлечено")

    def show_lsb_planes(self):
        if self.stego_image is None:
            messagebox.showerror("Ошибка", "Сначала создайте стеганоконтейнер")
            return

        array = np.array(self.stego_image)

        r = array[:, :, 0] & 1
        g = array[:, :, 1] & 1
        b = array[:, :, 2] & 1

        r_img = Image.fromarray((r * 255).astype(np.uint8))
        g_img = Image.fromarray((g * 255).astype(np.uint8))
        b_img = Image.fromarray((b * 255).astype(np.uint8))

        r_img.show(title="R LSB")
        g_img.show(title="G LSB")
        b_img.show(title="B LSB")


root = tk.Tk()
app = LSBSteganographyApp(root)
root.mainloop()