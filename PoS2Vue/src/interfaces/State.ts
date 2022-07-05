import type { Item, ItemModel } from "./Item";
import type { OrderModel } from "./Order";
import type { Receipt, ReceiptModel } from "./Receipt";
import type { Unit, UnitModel } from "./Unit";

export interface GlobalState {
    isModalHidden: boolean,
}

export interface ItemState {
    item: ItemModel,
    mode: string,
    editTarget?: number,
    currentUnitIndex?: number,
    items: Item[],
}

export interface PoSState {
    receipt: ReceiptModel,
    isModalHidden: boolean,
    isPreviewHidden: boolean,
    editTarget?: number,
    selectedItem: OrderModel,
}

export interface UnitState {
    unit: UnitModel,
    mode: string,
    editTarget?: number,
    currentUnitIndex?: number,
    units: Unit[],
}

export interface ReceiptState {
    receipts: Receipt[],
    receiptId?: number,
    startDate: string,
    endDate: string,
}