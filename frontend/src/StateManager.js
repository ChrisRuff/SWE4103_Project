
export var StateManager = {
    exampleData: null,
		classLayout: null,
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
		},
		setClassLayout(layout){
			this.classLayout = layout;
		},
		getClassLayout(){
			return this.classLayout
		}
		
}
