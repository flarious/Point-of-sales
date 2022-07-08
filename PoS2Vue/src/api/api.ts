import type { Item, ItemModel } from "@/interfaces/Item"
import type { Receipt, ReceiptDetail, ReceiptModel } from "@/interfaces/Receipt"
import type { ApiResponse } from "@/interfaces/Response"
import type { Unit, UnitModel } from "@/interfaces/Unit"
import axios from "axios"

const endpoint = "http://localhost:50000"

const endpointUnit = `${endpoint}/unit`
const endpointItem = `${endpoint}/item`
const endpointReceipt = `${endpoint}/receipt`   

export async function getUnitsList() {
    return await axios.get<ApiResponse<Unit[]>>(`${endpointUnit}/findAll`).then(response => response.data)
}

export async function addUnit(addValue: UnitModel) {
    return await axios.post<ApiResponse>(`${endpointUnit}/create`, addValue).then(response => response.data)
}

export async function editUnit(id: number, editValue: UnitModel) {
    return await axios.put<ApiResponse>(`${endpointUnit}/update/${id}`, editValue).then(response => response.data)
}

export async function deleteUnit(id: number) {
    return await axios.delete<ApiResponse>(`${endpointUnit}/delete/${id}`).then(response => response.data)
}

export async function getItemsList() {
    return await axios.get<ApiResponse<Item[]>>(`${endpointItem}/findAll`).then(response => response.data)
}

export async function addItem(newItem: ItemModel) {
    return await axios.post<ApiResponse>(`${endpointItem}/create`, newItem).then(response => response.data)
}   

export async function editItem(id: number, edittedItem: ItemModel) {
    return await axios.put<ApiResponse>(`${endpointItem}/update/${id}`, edittedItem).then(response => response.data);
}

export async function deleteItem(id: number) {
    return await axios.delete<ApiResponse>(`${endpointItem}/delete/${id}`).then(response => response.data)
}

export async function getReceiptsList() {
    return await axios.get<ApiResponse<Receipt[]>>(`${endpointReceipt}/findAll`).then(response => response.data)
}

export async function getReceiptByDate(start: string, end: string) {
    return await axios.get<ApiResponse<Receipt[]>>(`${endpointReceipt}/findByDateRange?start=${start}&end=${end}`).then(response => response.data)
}

export async function getOneReceipt(id: number) {
    return await axios.get<ApiResponse<ReceiptDetail>>(`${endpointReceipt}/findOne?id=${id}`).then(response => response.data)
}

export async function addReceipt(newReceipt: ReceiptModel) {
    return await axios.post<ApiResponse>(`${endpointReceipt}/create`, newReceipt).then(response => response.data)
}