import axios from "https://cdn.skypack.dev/axios"

const endpoint = "http://localhost:50000"

const endpointUnit = `${endpoint}/unit`
const endpointItem = `${endpoint}/item`
const endpointReceipt = `${endpoint}/receipt`
const endpointPos = `${endpoint}/pos`

export async function getUnitsList() {
    return await axios.get(`${endpointUnit}/findAll`).then(response => response.data)
}

export async function addUnit(addValue) {
    return await axios.post(`${endpointUnit}/create`, {name: addValue}).then(response => response.data)
}

export async function editUnit(id, editValue) {
    return await axios.put(`${endpointUnit}/update?id=${id}`, {name: editValue}).then(response => response.data)
}

export async function deleteUnit(id) {
    return await axios.delete(`${endpointUnit}/delete?id=${id}`).then(response => response.data)
}

export async function getItemsList() {
    return await axios.get(`${endpointItem}/findAll`).then(response => response.data)
}

export async function addItem(newItem) {
    return await axios.post(`${endpointItem}/create`, newItem).then(response => response.data)
}   

export async function editItem(id, edittedItem) {
    return await axios.put(`${endpointItem}/update?id=${id}`, edittedItem).then(response => response.data);
}

export async function deleteItem(id) {
    return await axios.delete(`${endpointItem}/delete?id=${id}`).then(response => response.data)
}

export async function getReceiptsList() {
    return await axios.get(`${endpointReceipt}/findAll`).then(response => response.data)
}

export async function getReceiptByDate(start, end) {
    return await axios.get(`${endpointReceipt}/findByDateRange?start=${start}&end=${end}`).then(response => response.data)
}

export async function getOneReceipt(id) {
    return await axios.get(`${endpointReceipt}/findOne?id=${id}`).then(response => response.data)
}

export async function addReceipt(newReceipt) {
    return await axios.post(`${endpointReceipt}/create`, newReceipt).then(response => response.data)
}