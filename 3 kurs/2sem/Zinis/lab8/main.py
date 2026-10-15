import time
import random
import math
import sys


def is_prime(n):
    if n < 2: return False
    if n == 2 or n == 3: return True
    if n % 2 == 0 or n % 3 == 0: return False
    i = 5
    while i * i <= n:
        if n % i == 0 or n % (i + 2) == 0:
            return False
        i += 6
    return True


def get_prime_factors(n):
    factors = set()
    while n % 2 == 0:
        factors.add(2)
        n //= 2
    i = 3
    while i * i <= n:
        while n % i == 0:
            factors.add(i)
            n //= i
        i += 2
    if n > 2:
        factors.add(n)
    return factors


def is_primitive_root(g, n):
    if math.gcd(g, n) != 1:
        return False

    phi = n - 1
    factors = get_prime_factors(phi)

    for factor in factors:
        if pow(g, phi // factor, n) == 1:
            return False
    return True


class ModularBenchmark:
    @staticmethod
    def generate_primes(start, end, count):
        primes = []
        candidates = list(range(start, end + 1))
        random.shuffle(candidates)

        for num in candidates:
            if is_prime(num):
                primes.append(num)
            if len(primes) == count:
                break
        return primes

    @staticmethod
    def run():
        x_values = ModularBenchmark.generate_primes(10 ^ 3, 10 ^ 100, 5)
        a_values = random.sample(range(5, 35), 2)
        n_values = [1024, 2048]

        print(f"{'a':<10} {'x (exp)':<12} {'n (mod)':<10} {'Result':<15} {'Time (ms)':<10}")
        print("-" * 75)

        for a in a_values:
            for x in x_values:
                for n in n_values:
                    start_time = time.perf_counter_ns()
                    y = pow(a, x, n)
                    end_time = time.perf_counter_ns()
                    elapsed_ms = (end_time - start_time) / 1_000_000

                    print(f"{a:<10} {x:<12} {n:<10} {y:<15} {elapsed_ms:.4f}")
        print("\n")


class RSAEncryption:
    def __init__(self):
        self.p = 18014398241046527
        self.q = 1298074214633706835075030044377087
        self.n = self.p * self.q
        self.phi = (self.p - 1) * (self.q - 1)
        self.e = self._generate_e()
        self.d = pow(self.e, -1, self.phi)

    def _generate_e(self):
        base = (self.p * self.q) // random.randint(1, 19)
        e = base
        while e > 0:
            if math.gcd(e, self.phi) == 1:
                return e
            e -= 1
        return 65537

    def encrypt(self, message):
        msg_bytes = message.encode('utf-8')
        msg_int = int.from_bytes(msg_bytes, byteorder='big')

        if msg_int >= self.n:
            raise ValueError("Сообщение слишком длинное для данного ключа RSA")

        return pow(msg_int, self.e, self.n)

    def decrypt(self, cypher_int):
        decrypted_int = pow(cypher_int, self.d, self.n)
        byte_len = (decrypted_int.bit_length() + 7) // 8
        return decrypted_int.to_bytes(byte_len, byteorder='big').decode('utf-8')

    @staticmethod
    def run(message):
        print("RSA:")
        rsa = RSAEncryption()

        print("Public Keys:")
        print(f" n: {rsa.n}")
        print(f" e: {rsa.e}")

        print(f"\nСообщение: {message}")

        t_start = time.perf_counter()
        encrypted = rsa.encrypt(message)
        t_enc = time.perf_counter() - t_start

        print(f"Зашифровано: {(encrypted)}")

        t_start = time.perf_counter()
        decrypted = rsa.decrypt(encrypted)
        t_dec = time.perf_counter() - t_start

        print(f"Расшифровано: {decrypted}")

        print(f"Время шифрования: {t_enc * 1000:.4f} ms")
        print(f"Время дешифрования: {t_dec * 1000:.4f} ms\n")


class ElGamalEncryption:
    def __init__(self):
        self.p = 433494437
        self.g = self._generate_g()
        self.x = random.randint(2, self.p - 2)
        self.y = pow(self.g, self.x, self.p)

    def _generate_g(self):
        start = self.p - random.randint(1, 1000)
        for g in range(start, 1, -1):
            if is_primitive_root(g, self.p):
                return g
        return 2

    def encrypt(self, message):
        encrypted_pairs = []
        for char in message:
            m = ord(char)
            k = random.randint(2, self.p - 2)
            while math.gcd(k, self.p - 1) != 1:
                k = random.randint(2, self.p - 2)

            a = pow(self.g, k, self.p)
            b = (pow(self.y, k, self.p) * m) % self.p
            encrypted_pairs.append((a, b))
        return encrypted_pairs

    def decrypt(self, pairs):
        decrypted_chars = []
        for a, b in pairs:
            s = pow(a, self.x, self.p)
            s_inv = pow(s, -1, self.p)
            m = (b * s_inv) % self.p
            decrypted_chars.append(chr(m))
        return "".join(decrypted_chars)

    @staticmethod
    def run(message):
        print("EL-GAMAL")
        eg = ElGamalEncryption()

        print("Параметры:")
        print(f" p: {eg.p}")
        print(f" g: {eg.g}")
        print(f" y (Public): {eg.y}")
        print(f" x (Private): {eg.x}")

        print(f"\nСообщение: {message}")

        t_start = time.perf_counter()
        enc_pairs = eg.encrypt(message)
        t_enc = time.perf_counter() - t_start

        enc_str = " ".join([f"{a} {b}" for a, b in enc_pairs])
        print(f"Зашифровано: {enc_str}")

        t_start = time.perf_counter()
        dec_msg = eg.decrypt(enc_pairs)
        t_dec = time.perf_counter() - t_start

        print(f"Расшифровано: {dec_msg}")

        print(f"Время шифрования: {t_enc * 1000:.4f} ms")
        print(f"Время дешифрования: {t_dec * 1000:.4f} ms\n")


def main():
    try:
        ModularBenchmark.run()
        message = "KazakovaVarvara"
        RSAEncryption.run(message)
        ElGamalEncryption.run(message)

    except KeyboardInterrupt:
        print("\nПрограмма остановлена пользователем.")


if __name__ == "__main__":
    main()