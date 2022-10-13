import numpy as np
import cv2


class Utils:
    @staticmethod
    def sparsify(label_vector, output_size):
        sparse_vector = []

        for label in label_vector:
            sparse_label = np.zeros(output_size)
            sparse_label[label] = 1

            sparse_vector.append(sparse_label)

        return np.array(sparse_vector)

    @staticmethod
    def get_preprocessed_img(img_path, image_size):
        img = cv2.imread(img_path)
        img = cv2.resize(img, (1366, 660))

        y = 0
        x = 220
        h = 660
        w = 1366 - x
        img2 = img[y:y + h, x:x + w]
        cv2.imwrite(img_path, img2)

        img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

        y = 0
        x = 220
        h = 660
        w = 1366 - x
        img = img[y:y + h, x:x + w]

        img = cv2.resize(img, (image_size, image_size))
        img = img.reshape(img.shape[0], img.shape[1], 1)
        img = img.astype('float32')
        img /= 255
        return img