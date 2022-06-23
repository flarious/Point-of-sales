import express from "express"
import path from "path"
import { fileURLToPath } from "url"

const __filename = fileURLToPath(import.meta.url)

const __dirname = path.dirname(__filename)

const app = express()

app.use(express.static('files'))

app.get("/", function(req, res) {
    res.redirect("/PointofSales")
})

app.get("/PointOfSales", function(req, res) {
    res.sendFile(path.join(__dirname + "/files" + "/home.html"))
})

app.get("/PointOfSales/unit", function(req, res) {
    res.sendFile(path.join(__dirname + "/files" + "/unit.html"))
})

app.get("/PointOfSales/item", function(req, res) {
    res.sendFile(path.join(__dirname + "/files" + "/item.html"))
})

app.get("/PointOfSales/receipt", function(req, res) {
    if (req.query.receiptId) {
        res.sendFile(path.join(__dirname + "/files" + "/receiptDetail.html"))
    }
    else {
        res.sendFile(path.join(__dirname + "/files" + "/receipt.html"))
    }
    
})

app.get("/PointOfSales/pos", function(req, res) {
    res.sendFile(path.join(__dirname + "/files" + "/pos.html"))
})


app.listen(3000, () => console.log("Server started"))