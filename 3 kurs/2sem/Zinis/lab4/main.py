import string
import math

ALPHABET = string.ascii_uppercase

# --- РОТОРЫ ---
rotors = {
    "V":   "VZBRGITYUPSDNHLXAWMJQOFECK",
    "VI":  "JPGVOUMFYQBENHZRDKASXLICTW",
    "VII": "NZJHGRCXMYSWBOUFAIVLPEKQDT"
}

# --- ОТРАЖАТЕЛЬ ---
reflectors = {
    "C_DUNN": {
        'A':'R','R':'A','B':'D','D':'B','C':'O','O':'C','E':'J','J':'E',
        'F':'N','N':'F','G':'T','T':'G','H':'K','K':'H','I':'V','V':'I',
        'L':'M','M':'L','P':'W','W':'P','Q':'Z','Z':'Q','S':'X','X':'S',
        'U':'Y','Y':'U'
    }
}

# --- ФУНКЦИИ ---
def encode_letter(letter, rotor, offset):
    i = (ALPHABET.index(letter) + offset) % 26
    return rotor[i]

def decode_letter(letter, rotor, offset):
    i = rotor.index(letter)
    i = (i - offset) % 26
    return ALPHABET[i]

def enigma(text, rotor_names, reflector_name, shifts, start_positions, detailed_first=False):
    rL = rotors[rotor_names[0]]
    rM = rotors[rotor_names[1]]
    rR = rotors[rotor_names[2]]

    offsetL, offsetM, offsetR = start_positions
    reflector = reflectors[reflector_name]
    result = ""
    step_counter = 0

    for ch_idx, ch in enumerate(text.upper()):
        if ch not in ALPHABET:
            continue

        step_counter += 1

        # --- шаг роторов через offset ---
        offsetR = (offsetR + shifts[2]) % 26
        if step_counter % 26 == 0:
            offsetM = (offsetM + shifts[1]) % 26
        if step_counter % (26*26) == 0:
            offsetL = (offsetL + shifts[0]) % 26

        detailed = detailed_first and ch_idx == 0
        if detailed:
            print(f"\nБуква: {ch}")

        # --- проход вперёд через роторы ---
        x = encode_letter(ch, rR, offsetR)
        if detailed: print(f"После R: {x}")
        x = encode_letter(x, rM, offsetM)
        if detailed: print(f"После M: {x}")
        x = encode_letter(x, rL, offsetL)
        if detailed: print(f"После L: {x}")

        # --- отражатель ---
        x = reflector[x]
        if detailed: print(f"После отражателя: {x}")

        # --- обратный проход через роторы ---
        x = decode_letter(x, rL, offsetL)
        if detailed: print(f"Назад через L: {x}")
        x = decode_letter(x, rM, offsetM)
        if detailed: print(f"Назад через M: {x}")
        x = decode_letter(x, rR, offsetR)
        if detailed: print(f"Назад через R: {x}")

        result += x

    return result

# --- ТЕКСТ ---
text = "Kazakova Varvara Vadimovna"

# --- ПАРАМЕТРЫ ---
rotor_order = ("V", "VI", "VII")
reflector = "C_DUNN"
shifts = (1, 2, 2)

# --- 5 разных установок ---
settings = [
    (0, 0, 0),
    (1, 2, 3),
    (5, 10, 15),
    (7, 7, 7),
    (25, 13, 2)
]

for idx, s in enumerate(settings):
    print("\n==============================")
    print(f"Начальная установка: {s}")

    detailed = (idx == 0)
    encrypted = enigma(text, rotor_order, reflector, shifts, s, detailed_first=detailed)
    print(f"\nРЕЗУЛЬТАТ: {encrypted}")

# --- ОЦЕНКА КРИПТОСТОЙКОСТИ ---
num_rotor_orders = 1  # порядок роторов фиксирован
num_rotor_positions = 26**3  # 26 позиций на каждом из 3 роторов
num_reflectors = 1  # фиксированный
num_stecker = 1  # нет коммутационной панели

total_keys = num_rotor_orders * num_rotor_positions * num_reflectors * num_stecker
bit_strength = math.log2(total_keys)

print("\n==============================")
print(f"Оценка криптостойкости учебной модели:")
print(f"Количество возможных ключей: {total_keys}")
print(f"Эквивалент битовой стойкости: {bit_strength:.1f} бит")