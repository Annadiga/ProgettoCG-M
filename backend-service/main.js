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
    var return_json = [];
    fs.readdir(path_to_watch, (err, files) => {
        return_json = files.map(file => {
            const splitted = file.split(".");
            return {
                name: splitted[0],
                extension: splitted[1] ? splitted[1] : "FOLDER",
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
