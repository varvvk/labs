from docx import Document
from bidict import bidict

spacings = (1.0, 1.1, 1.2, 1.3)
spacing_codes = bidict({
    1.0: '00',
    1.1: '01',
    1.2: '10',
    1.3: '11'
})


def to_bits(s):
    result = ''
    for c in s:
        bits = bin(ord(c))[2:]
        bits = '00000000'[len(bits):] + bits
        result += bits
    return result


def from_bits(bits):
    chars = []
    for b in range(int(len(bits) / 8)):
        byte = bits[b * 8:(b + 1) * 8]
        chars.append(chr(int(''.join([str(bit) for bit in byte]), 2)))
    return ''.join(chars)


def encode(message, container_path, output_path):
    doc = Document(container_path)
    bits = to_bits(message)

    if len(doc.paragraphs) < (int(len(bits) / 2)):
        raise Exception('Not enough paragraphs in container!')

    for para in doc.paragraphs:
        para.paragraph_format.line_spacing = 1.08

    i = 0
    while bits != '':
        doc.paragraphs[i].paragraph_format.line_spacing = spacing_codes.inverse[bits[:2]]
        bits = bits[2:]
        i += 1

    doc.save(output_path)


def decode(path):
    doc = Document(path)

    bits = ''
    for para in doc.paragraphs:
        if para.paragraph_format.line_spacing in spacings:
            bits += spacing_codes[para.paragraph_format.line_spacing]

    return from_bits(bits)


if __name__ == "__main__":
    encode('varya', 'container.docx', 'encoded.docx')
    print(decode('encoded.docx'))