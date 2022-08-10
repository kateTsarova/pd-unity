#!/usr/bin/env python
from __future__ import print_function
from __future__ import absolute_import
import sys

from os.path import basename
from classes.Vocabulary import *
from classes.Utils import *
from classes.model.pix2code import *
import os


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
trained_weights_path = os.path.join(script_dir, rel_path)
f = open("C:\\Users\\Nata\\Desktop\\myfile.txt", "w")
f.write(trained_weights_path)

trained_weights_path = 'D:\\again\\pix2code-master\\pix2code-master\\bin\\16\\'
trained_model_name = 'pix2code'
input_path = 'C:\\Users\\Nata\\Desktop\\out\\out.png'
output_path = 'C:\\Users\\Nata\\Desktop\\out\\'

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

with open("{}/{}.gui".format(output_path, "out"), 'w') as out_f:
    out_f.write(result.replace(START_TOKEN, "").replace(END_TOKEN, ""))
