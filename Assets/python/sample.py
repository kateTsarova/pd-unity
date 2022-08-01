#!/usr/bin/env python
from __future__ import print_function
from __future__ import absolute_import
__author__ = 'Tony Beltramelli - www.tonybeltramelli.com'
import sys

from os.path import basename
from classes.Sampler import *
from classes.model.pix2code import *

script_dir = os.path.dirname(__file__)
rel_path = "model/"
trained_weights_path = os.path.join(script_dir, rel_path)
f = open("C:\\Users\\Nata\\Desktop\\myfile.txt", "w")
f.write(trained_weights_path)

#trained_weights_path = 'D:\\again\\pix2code-master\\pix2code-master\\bin\\16\\'
trained_model_name = 'pix2code'
input_path = 'C:\\Users\\Nata\\Desktop\\out\\out.png'
# input_path = 'D:\\pix2code\\pix2code-master\\pix2code-master\\datasets\\pix2code_datasets\\web\\small\\0B660875-60B4-4E65-9793-3C7EB6C8AFD0.png'
# input_path = 'D:\\pix2code\\pix2code-master\\pix2code-master\\datasets\\pix2code_datasets\\web\\crop\\0\\0CE73E18-575A-4A70-9E40-F000B250344F.png'
# input_path = 'C:\\Users\\Nata\\Desktop\\d.png'
# output_path = 'C:\\Users\\Nata\\Desktop\\semestr 7\\praca dyplomowa\\att\\praca\\test\\out4'

# one row
# input_path = 'D:\\web_database\\all\\bklaqb.png'
# input_path = 'D:\\web_database\\all\\bwmiod.png'
# input_path = 'D:\\web_database\\all\\eohvzc.png'
# input_path = 'D:\\web_database\\all\\royfkr.png'

# two rows
# input_path = 'D:\\web_database\\all_two\\airmgs.png'
# input_path = 'D:\\web_database\\all_two\\aiwhst.png'
# input_path = 'D:\\web_database\\all_two\\aonglv.png'
# input_path = 'D:\\web_database\\all_two\\fubudi.png'

# two rows and check
# input_path = 'D:\\web_database\\all_checkpoint\\acbxwz.png'
# input_path = 'D:\\web_database\\all_checkpoint\\ackbpc.png'

# final
# input_path = 'D:\\web_database\\final\\abkmvq.png'
# input_path = 'D:\\web_database\\final\\adpxrv.png'

# input_path = 'D:\\again\\pix2code-master\\pix2code-master\\datasets\\pix2code_datasets\\web\\small\\1.png'
# input_path = 'D:\\again\\pix2code-master\\pix2code-master\\datasets\\pix2code_datasets\\web\\small\\2.png'

# input_path = 'D:\\web_database\\all\\abcvez.png'
# input_path = 'C:\\Users\\Nata\\Desktop\\plohjp.png'
# input_path2 = 'C:\\Users\\Nata\\Desktop\\aaeccv.png'
# input_path = 'C:\\Users\\Nata\\Desktop\\00E15BB2-5568-4466-BA18-A8A8D34FC61C.png'
# input_path = 'C:\\Users\\Nata\\Desktop\\0C8D1647-C0B7-43A3-AB68-AB4F8D7DC234.png'
# input_path = 'C:\\Users\\Nata\\Desktop\\aaeccv.png'
# input_path = 'D:\\again\\pix2code2-master\\pix2code2-master\\tests\\0C8D1647-C0B7-43A3-AB68-AB4F8D7DC234.png'
# input_path = 'D:\\again\\pix2code2-master\\pix2code2-master\\tests\\00CDC9A8-3D73-4291-90EF-49178E408797.png'
output_path = 'C:\\Users\\Nata\\Desktop\\out\\'
search_method = "greedy"
# search_method = 3

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
