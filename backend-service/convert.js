const fs = require('fs');
const path = require('path');
const pdf_to_img = require('pdf-to-img');
const JSZip = require('jszip');

async function generatePDFimages(pdf_folder, output_folder) {
    const zipper = new JSZip();
    // create output folder if not exists
    if (!fs.existsSync(output_folder)) {
        fs.mkdirSync(output_folder);
    }
    for (file of fs.readdirSync(pdf_folder)) {
        if (file.slice(-3) == 'pdf') {
            const pdf_zip_filename = path.join(output_folder, `${file.slice(0, -4)}.pdf`);
            // create output folder if not exists
            if (!fs.existsSync(pdf_zip_filename)) {
                console.log(`Starting conversion for ${pdf_zip_filename}`);
                let i = 0;
                for await (const page of await pdf_to_img.pdf(path.join(pdf_folder, file))) {
                    // make string of page number with leading zeros
                    const page_number = i.toString().padStart(3, '0');
                    // save page in zip as file
                    zipper.file(`page_${page_number}.png`, page);
                    i++;
                }
                console.log(`Conversion for ${pdf_zip_filename} finished, writing zip file to disk.`);
                zipper.generateNodeStream({ type: 'nodebuffer', streamFiles: true })
                    .pipe(fs.createWriteStream(pdf_zip_filename))
                    .on('finish', function () {
                        console.log(`${pdf_zip_filename} written.`);
                    });
            } else {
                console.log(`File ${pdf_zip_filename} already exists, skipping.`);
            }
        } else {
            console.log(`File ${file} is not a pdf, skipping.`);
        }
    }
}

SOURCE_FOLDER = "pdf";
OUTPUT_FOLDER = "zips";
generatePDFimages(SOURCE_FOLDER, OUTPUT_FOLDER);