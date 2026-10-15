import math

# ============================================
# 1. НОД (алгоритм Евклида)
# ============================================

def gcd_two(a, b):
    while b != 0:
        a, b = b, a % b
    return abs(a)

def gcd_three(a, b, c):
    return gcd_two(gcd_two(a, b), c)

# ============================================
# 2. Проверка числа на простоту
# ============================================

def is_prime(num):
    if num < 2:
        return False
    if num == 2:
        return True
    if num % 2 == 0:
        return False
    for i in range(3, int(math.sqrt(num)) + 1, 2):
        if num % i == 0:
            return False
    return True

# ============================================
# 3. Простые числа в интервале (обычный метод)
# ============================================

def primes_in_interval(start, end):
    return [x for x in range(start, end + 1) if is_prime(x)]

# ============================================
# 4. Решето Эратосфена
# ============================================

def sieve(n):
    sieve_list = [True] * (n + 1)
    sieve_list[0] = sieve_list[1] = False

    for i in range(2, int(math.sqrt(n)) + 1):
        if sieve_list[i]:
            for j in range(i * i, n + 1, i):
                sieve_list[j] = False

    return [i for i in range(2, n + 1) if sieve_list[i]]

# ============================================
# 5. Выполнение задания варианта
# ============================================

def run_variant():
    m = 667
    n = 703

    print("\n=== Задание 1: Интервал [2, n] ===")
    primes_2_n = sieve(n)
    print("Простые числа:", primes_2_n)
    print("Количество:", len(primes_2_n))

    approx = n / math.log(n)
    print("n/ln(n) ≈", round(approx, 2))

    print("\n=== Задание 2: Интервал [m, n] ===")
    primes_m_n = primes_in_interval(m, n)
    print("Простые числа:", primes_m_n)
    print("Количество:", len(primes_m_n))

# ============================================
# 6. Главное меню
# ============================================

def main():
    while True:
        print("\n==============================")
        print("1 - НОД двух чисел")
        print("2 - НОД трех чисел")
        print("3 - Простые числа в интервале")
        print("4 - Выполнить задание варианта (m=667, n=703)")
        print("0 - Выход")
        print("==============================")

        choice = input("Выберите пункт: ")

        if choice == "1":
            a = int(input("Введите первое число: "))
            b = int(input("Введите второе число: "))
            print("НОД =", gcd_two(a, b))

        elif choice == "2":
            a = int(input("Введите первое число: "))
            b = int(input("Введите второе число: "))
            c = int(input("Введите третье число: "))
            print("НОД =", gcd_three(a, b, c))

        elif choice == "3":
            start = int(input("Введите начало интервала: "))
            end = int(input("Введите конец интервала: "))
            primes = primes_in_interval(start, end)
            print("Простые числа:", primes)
            print("Количество:", len(primes))

        elif choice == "4":
            run_variant()

        elif choice == "0":
            print("Программа завершена.")
            break

        else:
            print("Неверный ввод!")

# ============================================
# Запуск программы
# ============================================

if __name__ == "__main__":
    main()