const express = require("express");
const path = require('path');
const fs = require('fs');

const app = express();
const static_path = path.join(__dirname, 'static');

app.use(express.static(static_path));

app.get("/", (req, res) => res.send("OK\n"));
app.get("/path", (req, res) => {
    const required_path = req.query.path;
    const path_to_watch = path.join(static_path, required_path);
    if (!fs.existsSync(path_to_watch) || !fs.statSync(path_to_watch).isDirectory()){
        res.sendStatus(404);
        console.warn("NOT OK PATH", path_to_watch);
        return;
    }
    fs.readdir(path_to_watch, (err, files) => {
        const return_json = files.map(file => {
            // if directory return name and extension FOLDER
            if (fs.statSync(path.join(path_to_watch, file)).isDirectory()) {
                return {
                    name: file,
                    extension: "FOLDER",
                };
            } 
            // if file without extension return name and empty extension
            const file_name = file.split('.');
            if (file_name.length == 1) {
                return {
                    name: file_name[0],
                    extension: "",
                };
            }
            // if file with extension return name and extension
            const file_extension = file_name.pop();
            return {
                name: file_name.join('.'),  // rejoin name cause extension was popped
                extension: file_extension,
            };
        });
        res.json(return_json);
        console.log("OK PATH", return_json);
    });
});

app.get("/QR", (req, res) => {
    const id = req.query.id;
    const res_str = `OK QR ${id}`;
    console.log(res_str);
    res.send(res_str);
});

app.listen(3000, () => console.log("Server started on port 3000"));
