export default {
    storage: window.localStorage,

    store (key: string, object: object) {
        if (!this._hasStorage()) { return }
        this.storage.setItem(key, JSON.stringify(object))
    },

    retrieve (key: string, defaultObject: object | null) {
        if (!this._hasStorage()) { return defaultObject }
        const data = this.storage.getItem(key)
        if (!data || data === null) return defaultObject
        return JSON.parse(data)
    },

    remove (key: string) {
        if (!this._hasStorage()) { return }
        this.storage.removeItem(key)
    },

    _hasStorage () {
        return this.storage && this.storage !== null
    }
}
