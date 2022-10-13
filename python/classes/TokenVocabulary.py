import sys
import numpy as np

START_TOKEN = "<START>"
END_TOKEN = "<END>"
PLACEHOLDER = " "
SEPARATOR = '->'


class TokenVocabulary:
    def __init__(self):
        self.binary_voc = {}
        self.voc = {}
        self.token_lookup = {}
        self.size = 0

        self.append(START_TOKEN)
        self.append(END_TOKEN)
        self.append(PLACEHOLDER)

    def append(self, token):
        if token not in self.voc:
            self.voc[token] = self.size
            self.token_lookup[self.size] = token
            self.size += 1

    def retrieve(self, path):
        file = open("{}/words.vocab".format(path), 'r')
        buffer = ""
        for line in file:
            try:
                separator_position = len(buffer) + line.index(SEPARATOR)
                buffer += line
                key = buffer[:separator_position]
                value = buffer[separator_position + len(SEPARATOR):]
                value = np.fromstring(value, sep=',')

                self.binary_voc[key] = value
                self.voc[key] = np.where(value == 1)[0][0]
                self.token_lookup[np.where(value == 1)[0][0]] = key

                buffer = ""
            except ValueError:
                buffer += line
        file.close()
        self.size = len(self.voc)
