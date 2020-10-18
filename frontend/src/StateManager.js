
export var StateManager = {
    exampleData: null,
		selectedClass: "--",
    setExampleData(data) {
        this.exampleData = data;
    },
    getExampleData() {
        return this.exampleData;
    },
		setSelectedClass(data){
			this.selectedClass = data;
		},
		getSelectedClass(){
			return this.selectedClass;
		}
}
