import type { Item, ItemModel, ItemDetail } from "./Item";
import type { Order, OrderModel } from "./Order";
import type { Receipt, ReceiptModel } from "./Receipt";
import type { Unit, UnitModel } from "./Unit";

export interface GlobalState {
    isModalHidden: boolean
}

export interface ItemState {
    item: ItemModel
    mode: string
    editTarget?: number
    currentUnitIndex?: number
    items: Item[]
}

export interface PosState {
    receipt: ReceiptModel
    isModalHidden: boolean
    isPreviewHidden: boolean
    editTarget?: number
    selectedItem: OrderModel
}

export interface PosFormState {
    modalItem: OrderModel
    selectedItem: ItemDetail
    items: Item[]
}

export interface UnitState {
    unit: UnitModel
    mode: string
    editTarget?: number
    currentUnitIndex?: number
    units: Unit[]
}

export interface ReceiptState {
    receipts: Receipt[]
    receiptId?: number
    startDate: string
    endDate: string
}