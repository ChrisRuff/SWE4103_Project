
export var StateManager = {
    accountState: null,
    setAccountState(state) {
        this.accountState = state;
    },
    getAccountState() {
        return this.accountState;
    }
}
