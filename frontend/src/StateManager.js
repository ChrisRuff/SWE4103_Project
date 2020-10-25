
export var StateManager = {
    exampleData: null,
    accountState: null,
    setAccountState(state) {
        this.accountState = state;
    },
    getAccountState() {
        return this.accountState;
    }
}
