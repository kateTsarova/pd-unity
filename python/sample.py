#!/usr/bin/env python
from __future__ import print_function
from __future__ import absolute_import
import sys

from os.path import basename
from classes.Vocabulary import *
from classes.Utils import *
# from classes.Compiler import *
from classes.model.pix2code import *
import os
import json


def predict_greedy(predict_model, voc, predict_output_size, context_length, input_img, require_sparse_label=True, sequence_length=150):
    current_context = [voc.vocabulary[PLACEHOLDER]] * (context_length - 1)
    current_context.append(voc.vocabulary[START_TOKEN])
    if require_sparse_label:
        current_context = Utils.sparsify(current_context, predict_output_size)

    predictions = START_TOKEN
    out_probas = []

    for i in range(0, sequence_length):

        probas = predict_model.predict(input_img, np.array([current_context]))
        prediction = np.argmax(probas)
        out_probas.append(probas)

        new_context = []
        for j in range(1, context_length):
            new_context.append(current_context[j])

        if require_sparse_label:
            sparse_label = np.zeros(predict_output_size)
            sparse_label[prediction] = 1
            new_context.append(sparse_label)
        else:
            new_context.append(prediction)

        current_context = new_context

        predictions += voc.token_lookup[prediction]

        if voc.token_lookup[prediction] == END_TOKEN:
            break

    return predictions, out_probas


script_dir = os.path.dirname(__file__)
rel_path = "model/"
result_path = "result/"
trained_weights_path = os.path.join(script_dir, rel_path)
# f = open("C:\\Users\\Nata\\Desktop\\myfile.txt", "w")
# f.write(trained_weights_path)

# trained_weights_path = 'D:\\again\\pix2code-master\\pix2code-master\\bin\\16\\'
trained_model_name = 'pix2code'
input_path = os.path.join(script_dir, result_path) + 'out.png'
output_path = os.path.join(script_dir, result_path)

meta_dataset = np.load("{}/meta_dataset.npy".format(trained_weights_path), allow_pickle=True)
input_shape = meta_dataset[0]
output_size = meta_dataset[1]

model = pix2code(input_shape, output_size, trained_weights_path)
model.load(trained_model_name)

vocabulary = Vocabulary()
vocabulary.retrieve(trained_weights_path)

file_name = basename(input_path)[:basename(input_path).find(".")]
evaluation_img = Utils.get_preprocessed_img(input_path, IMAGE_SIZE)

result, _ = predict_greedy(model, vocabulary, output_size, CONTEXT_LENGTH, np.array([evaluation_img]))
print("Result greedy: {}".format(result))

# f = open("C:\\Users\\Nata\\Desktop\\out\\demofile1.txt", "a")
# f.write("{}/{}.gui".format(output_path, "out"))
# f.close()

with open("{}/{}.gui".format(output_path, "out"), 'w') as out_f:
    out_f.write(result.replace(START_TOKEN, "").replace(END_TOKEN, ""))

# f = open("C:\\Users\\Nata\\Desktop\\out\\demofile2.txt", "a")
# f.write("2")
# f.close()













class Node:
    def __init__(self, key, parent_node, content_holder):
        self.key = key
        self.parent = parent_node
        self.children = []
        self.content_holder = content_holder

    def add_child(self, child):
        self.children.append(child)

    def show(self):
        print(self.key)
        for child in self.children:
            child.show()

    def render(self, mapping, rendering_function=None):
        content = ""
        for child in self.children:
            content += child.render(mapping, rendering_function)

        value = mapping[self.key]
        if rendering_function is not None:
            value = rendering_function(self.key, value)

        if len(self.children) != 0:
            value = value.replace(self.content_holder, content)

        return value



class Compiler:
    def __init__(self, dsl_mapping_file_path):
        a = dsl_mapping_file_path
        with open(dsl_mapping_file_path) as data_file:
            self.dsl_mapping = json.load(data_file)

        self.opening_tag = self.dsl_mapping["opening-tag"]
        self.closing_tag = self.dsl_mapping["closing-tag"]
        self.content_holder = self.opening_tag + self.closing_tag

        self.root = Node("body", None, self.content_holder)

    def compile(self, input_file_path, output_file_path, rendering_function=None):
        script_dir = os.path.dirname(__file__)
        rel_path = "model/"
        trained_weights_path = os.path.join(script_dir, rel_path)
        # f = open("C:\\Users\\Nata\\Desktop\\myfile.txt", "w")
        # f.write("hello")
        #
        dsl_file = open(input_file_path)
        current_parent = self.root

        for token in dsl_file:
            token = token.replace(" ", "").replace("\n", "")

            if token.find(self.opening_tag) != -1:
                token = token.replace(self.opening_tag, "")

                element = Node(token, current_parent, self.content_holder)
                current_parent.add_child(element)
                current_parent = element
            elif token.find(self.closing_tag) != -1:
                current_parent = current_parent.parent
            else:
                tokens = token.split(",")
                for t in tokens:
                    element = Node(t, current_parent, self.content_holder)
                    current_parent.add_child(element)

        output_html = self.root.render(self.dsl_mapping, rendering_function=rendering_function)
        f = open("C:\\Users\\Nata\\Desktop\\file.txt", "w")
        f.write(output_file_path)
        with open(output_file_path, 'w') as output_file:
            output_file.write(output_html)



input_file = input_path

FILL_WITH_RANDOM_TEXT = True
lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. "
TEXT_PLACE_HOLDER = "[]"

dsl_path = "D:\\again\\pix2code-master\\pix2code-master\\compiler\\assets\\web-dsl-mapping.json"



file_uid = basename(input_file)[:basename(input_file).find(".")]
path = output_path #"C:\\Users\\Nata\\Desktop\\out\\"  # input_file[:input_file.find(file_uid)]

f = open("C:\\Users\\Nata\\Desktop\\out\\demofile1.txt", "a")
f.write(file_uid)
f.close()

f = open("C:\\Users\\Nata\\Desktop\\out\\demofile2.txt", "a")
f.write(path)
f.close()

input_file_path = "{}{}.gui".format(path, file_uid)
output_file_path = "{}{}.html".format(path, file_uid)

compiler = Compiler(dsl_path)

f = open("C:\\Users\\Nata\\Desktop\\out\\demofile3.txt", "a")
f.write(input_file_path)
f.close()

f = open("C:\\Users\\Nata\\Desktop\\out\\demofile4.txt", "a")
f.write(output_file_path)
f.close()


def render_content_with_text(key, value):
    if FILL_WITH_RANDOM_TEXT:
        if key.find("btn") != -1:
            value = value.replace(TEXT_PLACE_HOLDER, lorem[:8])
        elif key.find("title") != -1:
            value = value.replace(TEXT_PLACE_HOLDER, lorem[:5])
        elif key.find("text") != -1:
            value = value.replace(TEXT_PLACE_HOLDER,
                                  lorem[:56])
        elif key.find("checkbox_active") != -1:
            value = value.replace(TEXT_PLACE_HOLDER,
                                  lorem[:8])
        elif key.find("checkbox_inactive") != -1:
            value = value.replace(TEXT_PLACE_HOLDER,
                                  lorem[:8])
    return value

compiler.compile(input_file_path, output_file_path, rendering_function=render_content_with_text)
