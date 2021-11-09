import React, { Component } from 'react';
import {getCats, getLifestyles, getCat,
    getCatBreeds, getCatBirthYears,
    addCat, deleteCat,
    editCat} from './CatModel.js';


class CatMain extends Component {
    constructor(props) {
        super(props);
        this.state = {
            selectedCat: "",
            currentName: "",
            currentColor: "",
            currentBreed: "",
            currentBirthYear: "",
            currentId: "",
            currentLifestyle: "",
            currentLifestyleId: 0,
            currentHabits: [],
            currentClick: "",
            currentView: "allCats",
            allCats: ["Inga katter"],
            lifestyles: [],
            currentCat: ""
        };
    }


    viewTitles() {
        return ["allCats", "catDetails", "newCat", "editCat"];
    }


    buttonTitles() {
        return ["Välj katt", "Ny katt", "Redigera katt", "Radera katt",
        "<<< Tillbaka", "Spara katt", "Uppdatera katt"];
    }



    allCatsChangeHandler = ({target}) => {
        this.setState({currentId: target.value, selectedCat: target.value.value});
    }

    formChangeHandler = ({target}) => {

        if(target.name === "currentLifestyleId") {
            this.setState({[target.name]: target.value,
                    currentLifestyle: target.options[target.selectedIndex].text});
        } else {
            this.setState({[target.name]: target.value});
        }
        this.setState({[target.name]: target.value});
    }


    clickHandler = ({target}) => {
        this.setState({currentClick: target.value});
        if(target.value === this.buttonTitles()[4]) {
            this.setState({currentId: "", currentView: this.viewTitles()[0]});
            this.clearSelectedCat();
            this.clearCurrentCatStates();

        } else if (target.value === this.buttonTitles()[1]) {
            this.setState({currentView: this.viewTitles()[2]});

        } else if (target.value === this.buttonTitles()[2]) {
            this.setCurrentCatStates();
            this.setState({currentView: this.viewTitles()[3]});

        } else if(target.value === this.buttonTitles()[3]) {
            const confirmDialog = window.confirm("Vill du verkligen radera katten " +
            this.state.currentCat.namn + "?");
            if(confirmDialog === true) {
                this.deleteCurrentCat(this.state.currentId);
            }
        }
    }


    async componentDidMount() {
        var batmanString = "";
        var res = "";
        for(var i = 0; i < 5; i++) {
            var j = parseInt("l");
            var k = j.toString().slice(0, -1);
            
           
            
                if(i === 4) {
                    k = " Batman"; 
                } 
            res += batmanString.concat(k);
        }
        console.log(res);

        const allCats = await getCats();
        const lifestyles = await getLifestyles();
        this.setState({
            allCats: allCats,
            lifestyles: lifestyles
        });
    }

    setCurrentCatStates() {
        this.setState({
            currentId: this.state.currentCat.id,
            currentName: this.state.currentCat.namn,
            currentBreed: this.state.currentCat.sort,
            currentColor: this.state.currentCat.farg,
            currentLifestyle: this.state.currentCat.livsstil.beskrivning,
            currentLifestyleId: this.state.currentCat.livsstil.livsstil_Id,
            currentHabits: this.state.currentCat.ovanor,
            currentBirthYear: this.state.currentCat.fodd,
        });
       
    }

    clearCurrentCatStates() {
        this.setState({
            currentId: "",
            currentName: "",
            currentBreed: "",
            currentBirthYear: "",
            currentLifestyleId: 0,
            currentColor: "",
            currentCat: "",
            currentHabits: [],
        })
    }

    clearSelectedCat() {
        this.setState({selectedCat: ""});
    }

    getCurrentCat(id) {
        getCat(id).then(response => {
            this.clearSelectedCat();
            this.setState({
                currentCat: response,
                currentView: this.viewTitles()[1]
            });
        });
    }


    errorMessage(errorCode, errorMessage) {
        return "Error code: " + errorCode + ". Error: " + errorMessage;
    }


    async addCurrentCat() {
        const catDetails = {namn: this.state.currentName,
            sort: this.state.currentBreed,
            fodd: parseInt(this.state.currentBirthYear),
            farg: this.state.currentColor,
            livsstil: {livsstil_Id: parseInt(this.state.currentLifestyleId), 
                beskrivning: this.state.currentLifestyle}
        };

        const response = await addCat(catDetails);

        if(response.namn) {
            alert("La till katten " + this.state.currentName
                + " i databasen.");
                this.clearCurrentCatStates();
                const allCats = await getCats();
                this.setState({
                    allCats: allCats,
                    currentView: this.viewTitles()[0]
                });
        } else {
            alert("Fel! Kunde inte lägga till katten " + this.state.currentName
                + " i databasen.\n" + this.errorMessage(response.status, response.title));
        } 
    }


    async editCurrentCat() {
        const catDetails = {id: this.state.currentId,
            namn: this.state.currentName,
            sort: this.state.currentBreed,
            fodd: parseInt(this.state.currentBirthYear),
            farg: this.state.currentColor,
            livsstil: {livsstil_Id: parseInt(this.state.currentLifestyleId), 
                beskrivning: this.state.currentLifestyle}
        };

        const response = await editCat(catDetails, catDetails.id);

        if(response.id) {
            alert("Uppdaterade katten " + this.state.currentName
                + " i databasen.");
                const allCats = await getCats();
                this.clearCurrentCatStates();
                this.setState({
                    allCats: allCats,
                    currentView: this.viewTitles()[0]
                });
        } else {
            alert("Fel! Kunde inte uppdatera katten " + this.state.currentName
                + " i databasen.\n" + this.errorMessage(response.status, response.title));
        }
    };
        
    

    async deleteCurrentCat(id) {
        let name = this.state.currentCat.namn;
        const response = await deleteCat(id);

        if(response === 1) {
            alert("Raderade katten " + name
                + " från databasen.");
                const allCats = await getCats();
                this.setState({
                    allCats: allCats,
                    currentView: this.viewTitles()[0]
                });
        } else {
            alert("Fel! Kunde inte radera katten " + this.state.currentName
                + " från databasen.\n" + this.errorMessage(response.status, response.title));
        }
    }


    submitHandler = (event) => {
        event.preventDefault();
        if(this.state.currentClick === this.buttonTitles()[0]) {
            if(this.state.currentId === "") {
                alert("Katt måste väljas!");
                return;
            }
            this.getCurrentCat(this.state.currentId);

        } else if(this.state.currentClick === this.buttonTitles()[5]) {
            this.addCurrentCat();
        } else if(this.state.currentClick === this.buttonTitles()[6]) {
            this.editCurrentCat();
        }
    }



    catDetailsView = () => {
        let habits;

        if(typeof this.state.currentCat.ovanor === 'undefined') {
            habits = "Inga kända";
        } else if(this.state.currentCat.ovanor.length === 0 || this.state.currentCat.ovanor === null) {
            habits = "Inga kända";
        } else {
            habits = this.state.currentCat.ovanor.map((habit, i, arr) => {
                if (arr.length - 1 === i) {
                    return habit.beteende; 
                } 
                return habit.beteende + ", "});
        }

        return <ul className="details-style-1">
        <h1 className="main-header-left">Detaljer för katten {this.state.currentCat.namn}</h1>
            <li>
                <label>Namn: </label><span>{this.state.currentCat.namn}</span>
            </li>
            <li>
                <label>Sort: </label><span>{this.state.currentCat.sort}</span>
            </li>
            <li>
                <label>Färg: </label><span>{this.state.currentCat.farg}</span>
            </li>
            <li>
                <label>Född: </label><span>{this.state.currentCat.fodd}</span>
            </li>
            <li>
                <label>Livsstil: </label><span>{this.state.currentCat.livsstil.beskrivning}</span>
            </li>
            <li>
                <label>Ovanor: </label><span>{habits}</span>
            </li>
            
            <li>
                <input className="grey-color space-right"
                type="submit"
                value={this.buttonTitles()[4]}
                onClick={this.clickHandler}
                />
                <input className="space-right"
                type="submit"
                value={this.buttonTitles()[2]}
                onClick={this.clickHandler}
                />
                <input className="brown-color"
                type="submit"
                value={this.buttonTitles()[3]}
                onClick={this.clickHandler}
                />
            </li>
        </ul>
    }



    allCatsView = () => {
        let startIndex = 0;

        return <form onSubmit={this.submitHandler}>
            <h1 className="main-header-center">Kattregistret: sök, ändra och lägg till!</h1>
                <ul className="form-style-1">
                    <li>
                        <label>Alla katter:</label>
                        <select value={this.state.selectedCat} className="field-select"
                            onChange={this.allCatsChangeHandler}>
                            <option key={startIndex} value={this.state.selectedCat}
                                disabled >Se katter i databasen...</option>
                            {this.state.allCats.map((cat) => <option key={startIndex++}
                                value={cat.id} > {cat.namn}, {cat.sort} </option>)}
                        </select>
                    </li>
                    <li>
                        <input className="space-right"
                        type="submit"
                        value={this.buttonTitles()[0]}
                        onClick={this.clickHandler}
                        />
                        <input className="brown-color"
                        type="submit"
                        value={this.buttonTitles()[1]}
                        onClick={this.clickHandler}
                        />
                    </li>
                </ul>
            </form>
        }



    catFormView = () => {
        let actionButtonText = "";
        let breedPlaceholder = null;
        let yearPlaceholder = null;
        let lifestylePlaceholder = null;
        let header = "";
        let startIndexBreed = 0;
        let startIndexYear = 0;
        let startIndexLifestyle = 0;

        if(this.state.currentView === this.viewTitles()[2]) {
            breedPlaceholder = <option key={startIndexBreed} value=""
                disabled >Se valbara raser...</option>;
            yearPlaceholder = <option key={startIndexYear} value=""
                disabled >Välj födelseår...</option>
            lifestylePlaceholder = <option key={startIndexLifestyle} value="0"
                disabled >Välj livsstil...</option>
            header = <h1 className="main-header-center">Lägg till ny katt i databasen</h1>
            actionButtonText = this.buttonTitles()[5]
        } else {
            header = <h1 className="main-header-center">Uppdatera katten {this.state.currentCat.name}</h1>
            actionButtonText = this.buttonTitles()[6]
        }
  

        return <form onSubmit={this.submitHandler}>
            {header}
                <ul className="form-style-1">
                    <li>
                        <label>Ange kattens namn: <span className="required">*</span></label>
                        <input name="currentName" value={this.state.currentName} type="text"
                            className="field-long" onChange={this.formChangeHandler} required />
                    </li>
                    <li>
                        <label>Välj sort:<span className="required">*</span></label>
                        <select name="currentBreed" value={this.state.currentBreed}
                            className="field-select" onChange={this.formChangeHandler} required>
                        {breedPlaceholder}
                        {getCatBreeds.map((breed) => <option key={startIndexBreed++}
                            value={breed} > {breed} </option>)}
                        </select>
                    </li>
                    <li>
                        <label>Ange färg <span className="required">*</span></label>
                        <input name="currentColor" type="text" value={this.state.currentColor}
                            className="field-long" onChange={this.formChangeHandler} required />
                    </li>
                    <li>
                        <label>Välj födelseår:<span className="required">*</span></label>
                        <select name="currentBirthYear" value={this.state.currentBirthYear}
                            className="field-select" onChange={this.formChangeHandler} required>
                        {yearPlaceholder}
                        {getCatBirthYears.map((year) => <option key={startIndexYear++}
                            value={year} > {year} </option>)}
                        </select>
                    </li>
                    <li>
                 
                        <label>Välj livsstil:<span className="required">*</span></label>
                        <select name="currentLifestyleId" value={this.state.currentLifestyleId}
                            className="field-select" onChange={this.formChangeHandler} required>
                        {lifestylePlaceholder}
                        {this.state.lifestyles.map((ls) => <option key={startIndexLifestyle++}
                            value={ls.livsstil_Id} > {ls.beskrivning} </option>)}
                        </select>
                    </li>
                    <li>
                        <input className="grey-color space-right"
                        type="submit"
                        value={this.buttonTitles()[4]}
                        onClick={this.clickHandler}
                        />
                    <input className="space-right"
                        type="submit"
                        value={actionButtonText}
                        onClick={this.clickHandler}
                        />
                    </li>
                </ul>
            </form>
            }


        render() {
            let view = "";

            if(this.state.currentView === this.viewTitles()[0]) {
                view = this.allCatsView();
            } else if(this.state.currentView === this.viewTitles()[1]) {
                view = this.catDetailsView();
            } else if(this.state.currentView === this.viewTitles()[2] ||
            this.state.currentView === this.viewTitles()[3]) {
                view = this.catFormView();
            }

            return (
                view
            );
        }
    }

    export default CatMain;
