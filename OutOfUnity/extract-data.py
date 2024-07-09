from PIL import Image
import os
import colorsys


def calculate_average_hue(image_path):
    with Image.open(image_path) as img:
        # Convert image to RGB if it's not
        img = img.convert('RGB')

        # Calculate the average hue
        pixels = list(img.getdata())
        total_hue = 0
        for pixel in pixels:
            # Normalize the RGB values to range [0, 1]
            r, g, b = [x / 255.0 for x in pixel]
            # Convert RGB to HSV
            h, s, v = colorsys.rgb_to_hsv(r, g, b)
            total_hue += h

        avg_hue = total_hue / len(pixels)
        return avg_hue


# Directory containing images
image_directory = './laion2B-en-aesthetic-minified-main/images'


with open("./data.csv", "w") as file:

    file.write("Name;Width;Height;Average Red;Average Green;Average Blue;Average Hue;Average Saturation;Average Value;\n")

    # Iterate over each image in the directory
    for image_file in os.listdir(image_directory):
        image_path = os.path.join(image_directory, image_file)

        with Image.open(image_path) as img:
            # Convert image to RGB if it's not
            img = img.convert('RGB')

            width, height = img.size

            # Calculate the average hue
            pixels = list(img.getdata())
            total_r = 0
            total_g = 0
            total_b= 0
            total_hue = 0
            total_sat = 0
            total_val = 0
            for pixel in pixels:
                # Normalize the RGB values to range [0, 1]
                r, g, b = [x / 255.0 for x in pixel]
                total_r += r
                total_g += g
                total_b += b
                # Convert RGB to HSV
                h, s, v = colorsys.rgb_to_hsv(r, g, b)
                total_hue += h
                total_sat += s
                total_val += v

            avg_r = total_r / len(pixels)
            avg_g = total_g / len(pixels)
            avg_b = total_b / len(pixels)
            avg_hue = total_hue / len(pixels)
            avg_sat = total_sat / len(pixels)
            avg_val = total_val / len(pixels)

            file.write(f"{image_file};{width};{height};{avg_r};{avg_g};{avg_b};{avg_hue};{avg_sat};{avg_val};\n")
