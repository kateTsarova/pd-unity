#!/usr/bin/env python
from __future__ import print_function
from __future__ import absolute_import
import sys

from os.path import basename
from classes.TokenVocabulary import *
from classes.Utils import *
from classes.PageCompiler import *
from classes.model.page_from_scratch import *
import os


def predict_greedy(predict_model, voc, predict_output_size, context_length, input_img, require_sparse_label=True, sequence_length=150):
    current_context = [voc.voc[PLACEHOLDER]] * (context_length - 1)
    current_context.append(voc.voc[START_TOKEN])
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
result_path = sys.argv[1]
trained_weights_path = os.path.join(script_dir, rel_path)

trained_model_name = 'page_from_scratch'
input_path = result_path + '/out.png'
output_path = result_path

meta_dataset = np.load("{}/meta_dataset.npy".format(trained_weights_path), allow_pickle=True)
input_shape = meta_dataset[0]
output_size = meta_dataset[1]

model = page_from_scratch(input_shape, output_size, trained_weights_path)
model.load(trained_model_name)

vocabulary = TokenVocabulary()
vocabulary.retrieve(trained_weights_path)

file_name = basename(input_path)[:basename(input_path).find(".")]
evaluation_img = Utils.get_preprocessed_img(input_path, IMAGE_SIZE)

result, _ = predict_greedy(model, vocabulary, output_size, CONTEXT_LENGTH, np.array([evaluation_img]))

with open("{}/{}.gui".format(output_path, "out"), 'w') as out_file:
    out_file.write(result.replace(START_TOKEN, "").replace(END_TOKEN, ""))

dsl_path = "{}/web-dsl-mapping.json".format(trained_weights_path)

input_file_path = "{}/{}.gui".format(output_path, "out")
output_file_path = "{}/{}.html".format(output_path, "out")

page_compiler = PageCompiler(dsl_path)

page_compiler.compile(input_file_path, output_file_path)
