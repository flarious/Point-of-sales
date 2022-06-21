const html = `
<h3>เลือกสินค้า</h3>
<hr>
<table id="item_options">
</table>
<hr>
<p id="placeholder">No Item Selected</p>
<div id="selected_item" class="hide">
    <h1>Item detail</h1>
    <h3>รหัสสินค้า</h3>
    <p id="selected_item_code"></p>
    <h3>ชื่อสินค้า</h3>
    <p id="selected_item_name"></p>
    <h3>ราคา</h3>
    <p id="selected_item_price"></p>
</div>
<hr>
<button class="close">close</button><br>
<button class="save">save changes</button>
`

export const modal = function(modalElement) {
    let _items
    let _index
    let _callback
    let _target
    modalElement.classList.add("hide")
    modalElement.innerHTML = html
    
    modalElement.querySelector(".save").addEventListener("click", save)
    modalElement.querySelector(".close").addEventListener("click", close)

    function setItemList(items) {
        _items = items
        for (const item of _items) {
            const newItem = document.createElement("tr")
            var template = document.querySelector("#option_row")

            var clone = template.content.cloneNode(true)
            clone.querySelector(".option_name").innerText = item.name

            newItem.dataset.id = item.id
            newItem.addEventListener("click", function() {
                select(event.currentTarget.closest("tr").rowIndex)
            })
            newItem.append(clone)
            modalElement.querySelector("#item_options").appendChild(newItem)
        }
    }

    function select(index) {
        _index = index
        highlightItem()
        changeItem()
    }

    function highlightItem() {
        // modalElement.querySelector("#item_options").querySelectorAll("tr").forEach(row => {
        //     if (row.rowIndex != _index) {
        //         row.style.color = "black"
        //     }
        //     else {
        //         row.style.color = "blue"
        //     }
        // })

        const rows = modalElement.querySelector("#item_options").querySelectorAll("tr")

        for (const row of rows) {
            if (row.rowIndex != _index) {
                row.style.color = "black"
            }
            else {
                row.style.color = "blue"
            }
        }
    }

    function clearHighlight() {
        // modalElement.querySelector("#item_options").querySelectorAll("tr").forEach(row => row.style.color = "black")

        const rows = modalElement.querySelector("#item_options").querySelectorAll("tr")

        for (const row of rows) {
            row.style.color = "black"
        }
    }

    function changeItem() {
        modalElement.querySelector("#placeholder").classList.add("hide")
        modalElement.querySelector("#selected_item").classList.remove("hide")

        document.querySelector("#selected_item_code").innerText = _items[_index].code
        document.querySelector("#selected_item_name").innerText = _items[_index].name
        document.querySelector("#selected_item_price").innerText = _items[_index].price
    }

    function open(event, onSaveCallBack) {
        if (event.currentTarget.innerText != "เลือกสินค้า") {
            select(_items.findIndex(item => item.id == event.currentTarget.dataset.id))
        }
        else {
            clearHighlight()
        }
        _callback = onSaveCallBack
        _target = event.currentTarget.closest("tr").rowIndex - 1
        modalElement.classList.remove("hide")
        focusForm()
        
    }

    function close() {
        modalElement.classList.add("hide")
        unfocusForm()
    }

    function focusForm() {
        document.body.style.backgroundColor = "lightgray"
        // document.body.querySelectorAll("#item_select, #item_select *").forEach(
        //     elem => elem.style.backgroundColor = "white"
        // )
        // document.body.querySelectorAll("*:not(#item_select, #item_select *)").forEach(
        //     elem => { 
        //         elem.style.pointerEvents = "none" 
        //         elem.style.opacity = 0.5
        //     }
        // )

        const styleFrom = document.body.querySelectorAll("#item_select, #item_select *")

        for (const elem of styleFrom) {
            elem.style.backgroundColor = "white"
        }

        const styleNotForm = document.body.querySelectorAll("*:not(#item_select, #item_select *)")

        for (const elem of styleNotForm) {
            elem.style.pointerEvents = "none" 
            elem.style.opacity = 0.5
        }
    }

    function unfocusForm() {
        document.body.style.backgroundColor = "white"
        // document.body.querySelectorAll("*:not(#item_select, #item_select *)").forEach(
        //     elem => { 
        //         elem.style.pointerEvents = "" 
        //         elem.style.opacity = 1
        //     }
        // )

        const styleNotForm = document.body.querySelectorAll("*:not(#item_select, #item_select *)")

        for (const elem of styleNotForm) {
            elem.style.pointerEvents = "none" 
            elem.style.opacity = 0.5
        }
    }

    function save() {
        close()
        _callback(_items[_index], _target)
    }

    return {
        open: open,
        init: setItemList,
    }
}