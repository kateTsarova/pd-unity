#!/usr/bin/env python
from __future__ import print_function
from __future__ import absolute_import
import sys

from os.path import basename
from classes.Sampler import *
from classes.Compiler import *
from classes.model.pix2code import *
import os

script_dir = os.path.dirname(__file__)
rel_path = "model/"
result_path = "result/"
trained_weights_path = os.path.join(script_dir, rel_path)
# f = open("C:\\Users\\Nata\\Desktop\\myfile.txt", "w")
# # f.write(trained_weights_path)
# f.write("lalala")

# trained_weights_path = 'D:\\again\\pix2code-master\\pix2code-master\\bin\\16\\'
# trained_weights_path = 'D:\\again\\pix2code-master\\pix2code-master\\bin\\20\\'
trained_model_name = 'pix2code'
input_path = 'C:\\Users\\Nata\\Desktop\\out\\out.png'
output_path = os.path.join(script_dir, rel_path)
search_method = "greedy"

meta_dataset = np.load("{}/meta_dataset.npy".format(trained_weights_path), allow_pickle=True)
input_shape = meta_dataset[0]
output_size = meta_dataset[1]

model = pix2code(input_shape, output_size, trained_weights_path)
model.load(trained_model_name)

sampler = Sampler(trained_weights_path, input_shape, output_size, CONTEXT_LENGTH)

file_name = basename(input_path)[:basename(input_path).find(".")]
evaluation_img = Utils.get_preprocessed_img(input_path, IMAGE_SIZE)

if search_method == "greedy":
    result, _ = sampler.predict_greedy(model, np.array([evaluation_img]))
    print("Result greedy: {}".format(result))
else:
    beam_width = int(search_method)
    print("Search with beam width: {}".format(beam_width))
    result, _ = sampler.predict_beam_search(model, np.array([evaluation_img]), beam_width=beam_width)
    print("Result beam: {}".format(result))

with open("{}/{}.gui".format(output_path, "out"), 'w') as out_f:
    out_f.write(result.replace(START_TOKEN, "").replace(END_TOKEN, ""))




# input_file = "C:\\Users\\Nata\\Desktop\\out\\out.png"
#
# FILL_WITH_RANDOM_TEXT = True
# lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. "
# TEXT_PLACE_HOLDER = "[]"
#
# dsl_path = "D:\\again\\pix2code-master\\pix2code-master\\compiler\\assets\\web-dsl-mapping.json"
# compiler = Compiler(dsl_path)
#
#
# def render_content_with_text(key, value):
#     if FILL_WITH_RANDOM_TEXT:
#         if key.find("btn") != -1:
#             value = value.replace(TEXT_PLACE_HOLDER, lorem[:8])
#         elif key.find("title") != -1:
#             value = value.replace(TEXT_PLACE_HOLDER, lorem[:5])
#         elif key.find("text") != -1:
#             value = value.replace(TEXT_PLACE_HOLDER,
#                                   lorem[:56])
#         elif key.find("checkbox_active") != -1:
#             value = value.replace(TEXT_PLACE_HOLDER,
#                                   lorem[:8])
#         elif key.find("checkbox_inactive") != -1:
#             value = value.replace(TEXT_PLACE_HOLDER,
#                                   lorem[:8])
#     return value
#
# file_uid = basename(input_file)[:basename(input_file).find(".")]
# path = "C:\\Users\\Nata\\Desktop\\out\\"  # input_file[:input_file.find(file_uid)]
#
# input_file_path = "{}{}.gui".format(path, file_uid)
# output_file_path = "{}{}.html".format(path, file_uid)
#
# f = open("C:\\Users\\Nata\\Desktop\\out\\demofile2.txt", "a")
# f.write("Now the file has more content!")
# f.close()
#
# compiler.compile(input_file_path, output_file_path, rendering_function=render_content_with_text)
