import time
import struct
import math
from typing import Union, List, Tuple, Optional

class DES:
    # --- Таблицы (без изменений, они стандартные) ---
    IP = [
        58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4,
        62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8,
        57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3,
        61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7
    ]
    IP_INV = [
        40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31,
        38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29,
        36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27,
        34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25
    ]
    E = [
        32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13,
        12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25,
        24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1
    ]
    P = [
        16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10,
        2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25
    ]
    S_BOXES = [
        [[14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7], [0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8], [4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0], [15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13]],
        [[15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10], [3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5], [0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15], [13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9]],
        [[10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8], [13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1], [13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7], [1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12]],
        [[7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15], [13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9], [10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4], [3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14]],
        [[2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9], [14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6], [4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14], [11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3]],
        [[12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11], [10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8], [9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6], [4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13]],
        [[4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1], [13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6], [1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2], [6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12]],
        [[13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7], [1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2], [7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8], [2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11]]
    ]
    SHIFT_SCHEDULE = [1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1]
    PC1 = [
        57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36,
        63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4
    ]
    PC2 = [
        14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2,
        41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32
    ]
    WEAK_KEYS = ["0101010101010101", "1F1F1F1F1F1F1F1F", "E0E0E0E0F1F1F1F1", "FEFEFEFEFEFEFEFE"]
    SEMIWEAK_KEYS = [
        ("01FE01FE01FE01FE", "FE01FE01FE01FE01"), ("1FE01FE00EF10EF1", "E01FE01FF10EF10E"),
        ("01E001E001F101F1", "E001E001F101F101"), ("1FFE1FFE0EFE0EFE", "FE1FFE1FFE0EFE0E"),
        ("011F011F010E010E", "1F011F010E010E01"), ("E0FEE0FEF1FEF1FE", "FEE0FEE0FEF1FEF1")
    ]

    def __init__(self, key: bytes):
        if len(key) != 8: raise ValueError(f"Ключ должен быть 8 байт, получено {len(key)}")
        self.key = key
        self.subkeys = self._generate_subkeys(key)

    def _permute(self, data: int, table: List[int], n_in: int, n_out: int) -> int:
        result = 0
        for pos in table:
            bit = (data >> (n_in - pos)) & 1
            result = (result << 1) | bit
        return result

    def _generate_subkeys(self, key: bytes) -> List[int]:
        key_int = int.from_bytes(key, byteorder='big')
        key56 = self._permute(key_int, self.PC1, 64, 56)
        c, d = (key56 >> 28) & 0xFFFFFFF, key56 & 0xFFFFFFF
        subkeys = []
        for i in range(16):
            shift = self.SHIFT_SCHEDULE[i]
            c = ((c << shift) | (c >> (28 - shift))) & 0xFFFFFFF
            d = ((d << shift) | (d >> (28 - shift))) & 0xFFFFFFF
            subkeys.append(self._permute((c << 28) | d, self.PC2, 56, 48))
        return subkeys

    def _f_function(self, r: int, subkey: int) -> int:
        r_exp = self._permute(r, self.E, 32, 48)
        x = r_exp ^ subkey
        result = 0
        for i in range(8):
            block = (x >> (42 - i * 6)) & 0x3F
            row = ((block >> 4) & 0x2) | (block & 0x1)
            col = (block >> 1) & 0xF
            result = (result << 4) | self.S_BOXES[i][row][col]
        return self._permute(result, self.P, 32, 32)

    def _process_block(self, block: int, decrypt: bool = False) -> int:
        block = self._permute(block, self.IP, 64, 64)
        l, r = (block >> 32) & 0xFFFFFFFF, block & 0xFFFFFFFF
        keys = self.subkeys[::-1] if decrypt else self.subkeys
        for subkey in keys:
            l, r = r, l ^ self._f_function(r, subkey)
        return self._permute((r << 32) | l, self.IP_INV, 64, 64)

    def _pad(self, data: bytes) -> bytes:
        pad_len = 8 - (len(data) % 8)
        return data + bytes([pad_len] * pad_len)

    def _unpad(self, data: bytes) -> bytes:
        if not data: return data
        pad_len = data[-1]
        return data[:-pad_len] if 0 < pad_len <= 8 else data

    def encrypt(self, data: bytes, avalanche_analysis: bool = False) -> Tuple[bytes, dict]:
        padded = self._pad(data)
        res = bytearray()
        analysis = {}
        if avalanche_analysis:
            b1 = int.from_bytes(padded[:8], 'big')
            b2 = b1 ^ (1 << 63)
            e1, e2 = self._process_block(b1), self._process_block(b2)
            changed = bin(e1 ^ e2).count("1")
            analysis = {
                'original_block': f"{b1:016X}", 'modified_block': f"{b2:016X}",
                'encrypted_original': f"{e1:016X}", 'encrypted_modified': f"{e2:016X}",
                'changed_bits': changed, 'avalanche_ratio': changed / 64.0
            }
        for i in range(0, len(padded), 8):
            blk = int.from_bytes(padded[i:i+8], 'big')
            res.extend(self._process_block(blk).to_bytes(8, 'big'))
        return bytes(res), analysis

    def decrypt(self, data: bytes) -> bytes:
        res = bytearray()
        for i in range(0, len(data), 8):
            blk = int.from_bytes(data[i:i+8], 'big')
            res.extend(self._process_block(blk, True).to_bytes(8, 'big'))
        return self._unpad(bytes(res))

    @staticmethod
    def key_to_hex(key: bytes) -> str: return key.hex().upper()

    @staticmethod
    def is_weak_key(key: bytes) -> Tuple[bool, str]:
        kh = key.hex().upper()
        for w in DES.WEAK_KEYS:
            if kh == w: return True, "слабый ключ"
        for p1, p2 in DES.SEMIWEAK_KEYS:
            if kh in (p1, p2): return True, "полуслабый ключ"
        return False, "нормальный ключ"

    @staticmethod
    def create_key_from_string(s: str) -> bytes:
        return s.encode('utf-8').ljust(8, b'\x00')[:8]

class TripleDES:
    def __init__(self, key: bytes):
        self.cipher = DES(key)
    def encrypt(self, data: bytes, avalanche: bool = False):
        # EDE
        e1, analysis = self.cipher.encrypt(data, avalanche)
        d2 = self.cipher.decrypt(e1)
        e3, _ = self.cipher.encrypt(d2)
        return e3, analysis
    def decrypt(self, data: bytes):
        # DED
        d1 = self.cipher.decrypt(data)
        e2, _ = self.cipher.encrypt(d1)
        d3 = self.cipher.decrypt(e2)
        return d3

# --- Аналитические функции ---

def speed_test(cipher, data: bytes, iterations: int = 100) -> dict:
    s = time.perf_counter()
    for _ in range(iterations): enc, _ = cipher.encrypt(data)
    et = time.perf_counter() - s
    enc, _ = cipher.encrypt(data)
    s = time.perf_counter()
    for _ in range(iterations): cipher.decrypt(enc)
    dt = time.perf_counter() - s
    total_kb = (len(data) * iterations) / 1024
    return {'et': et, 'dt': dt, 'es': total_kb / et, 'ds': total_kb / dt}

def compression_analysis(orig: bytes, enc: bytes) -> dict:
    def ent(d: bytes):
        if not d: return 0
        freq = {}
        for b in d: freq[b] = freq.get(b, 0) + 1
        return -sum((c/len(d)) * math.log2(c/len(d)) for c in freq.values()) / 8
    e_o, e_e = ent(orig), ent(enc)
    return {'e_o': e_o, 'e_e': e_e, 'c_o': 1-e_o, 'c_e': 1-e_e}

def main():
    key_str = "VARVARA"
    key = DES.create_key_from_string(key_str)
    print(f"\nКлюч: '{key_str}' | HEX: {key.hex().upper()}")
    _, kt = DES.is_weak_key(key)
    print(f"Статус ключа: {kt}")

    plaintext = b"KAZEEEAKOVA HSOWMAJDEN URBCKS hwend yaiq"
    print(f"\n[2] Исходный текст ({len(plaintext)} байт): {plaintext}")

    tdes = TripleDES(key)
    encrypted, avalanche = tdes.encrypt(plaintext, avalanche=True)
    print(f"\n[3] Шифрование выполнено")

    if avalanche:
        print(f"\n[4] Анализ лавинного эффекта")
        print(f"Оригинал блок: {avalanche['original_block']}")
        print(f"Модиф. блок:  {avalanche['modified_block']}")
        print(f"Изменилось битов: {avalanche['changed_bits']} из 64 ({avalanche['avalanche_ratio']*100:.1f}%)")

    decrypted = tdes.decrypt(encrypted)
    print(f"\n[5] Дешифрование: {decrypted}")
    print(f"Целостность: {plaintext == decrypted}")

    st = speed_test(tdes, plaintext)
    print(f"\n[6] Тест скорости ({len(plaintext)*100} байт):")
    print(f"Шифрование: {st['es']:.2f} KB/s | Дешифрование: {st['ds']:.2f} KB/s")

    ca = compression_analysis(plaintext, encrypted)
    print(f"\n[7] Анализ сжатия (энтропия):")
    print(f"Оригинал: {ca['e_o']:.3f} (Сжимаемость: {ca['c_o']*100:.1f}%)")
    print(f"Шифртекст: {ca['e_e']:.3f} (Сжимаемость: {ca['c_e']*100:.1f}%)")

    print("\n[8] Проверка слабых ключей из Таблицы 5.3:")
    for wk_hex in DES.WEAK_KEYS:
        wk = bytes.fromhex(wk_hex)
        wc = DES(wk)
        p_int = int.from_bytes(b"DATA64BT", 'big')
        e1_int = wc._process_block(p_int)
        e2_int = wc._process_block(e1_int)
        is_reversible = (p_int == e2_int)
        print(f"Ключ {wk_hex}:")
        print(f"  P:      {p_int:016X}")
        print(f"  E(E(P)): {e2_int:016X} (обратимо: {is_reversible})")

    print(f"\n[10] проверка полуслабых ключей из таблицы 5.4")
    for i, (key1_hex, key2_hex) in enumerate(DES.SEMIWEAK_KEYS, 1):
        try:
            key1 = bytes.fromhex(key1_hex)
            key2 = bytes.fromhex(key2_hex)
            cipher1 = DES(key1)
            cipher2 = DES(key2)

            enc1, _ = cipher1.encrypt(b"AAAAAAAA")
            enc2, _ = cipher2.encrypt(b"AAAAAAAA")

            print(f"Пара {i}:")
            print(f"  K1: {key1_hex} -> {enc1.hex().upper()}")
            print(f"  K2: {key2_hex} -> {enc2.hex().upper()}")
            print(f"  Совпадают: {enc1 == enc2}")
        except Exception as e:
            print(f"Пара {i}: ошибка - {e}")
if __name__ == "__main__":
    main()