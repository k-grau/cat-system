function getBaseUrl() {
    return "https://localhost:5001/cat";
}


export const getCats = () => {
    return new Promise ((resolve) => {
        const url = getBaseUrl();
        fetch(url)
        .then((response) => response.json())
        .then(data => {
            resolve(data);
        })
        .catch((error) => {
            console.log("Error: ", error)
        });
    });
}


export const getCat = (id) => {
    return new Promise ((resolve) => {
        const url = getBaseUrl() + "/" + id;
        const params = {
           
        }
        fetch(url, params)
        .then((response) => response.json())
        .then(data => {
            resolve(data);
        })
        .catch((error) => {
            console.log("Error: ", error)
        });
    });
}



export const addCat = (catDetails) => {
    return new Promise ((resolve) => {
        const url = getBaseUrl();
        const params={
            headers: {
                'content-type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify(catDetails)
        };
        fetch(url, params)
        .then((response) => response.json())
        .then(data => {
            console.log(data)
            resolve(data);
        })
        .catch((error) => {
            console.log("Error: ", error)
        });
    })
}



export const deleteCat = (id) => {
    return new Promise ((resolve) => {
        const url = getBaseUrl() + "/" + id;
        console.log(url);
        console.log(id);
        const params={
            headers: {
                'content-type': 'application/json',
            },
            method: "DELETE"
        }
        fetch(url, params)
        .then((response) => response.json())
        .then(data => {

            resolve(data);
        })
        .catch((error) => {
            console.log("Error: ", error)
        });
    });
}




export const editCat = (catDetails, id) => {
    return new Promise ((resolve) => {
        const url = getBaseUrl() + "/" + id;
        const params={
            headers: {
                'content-type': 'application/json'
            },
            body: JSON.stringify(catDetails),
            method: "PUT"
        }
        fetch(url, params)
        .then((response) => response.json())
        .then(data => {
            resolve(data);
        })
        .catch((error) => {
            console.log("Error: ", error)
        });
    });
}



export const getLifestyles = () => {
    return new Promise ((resolve) => {
        const url = getBaseUrl() + "/lifestyles/";
        fetch(url)
        .then((response) => response.json())
        .then(data => {
            resolve(data);
        })
        .catch((error) => {
            console.log("Error: ", error)
        });
    });
}


export const getCatBreeds = ["Austrailian Mist", "Maine coon", "Rex", "Siames",
"Bengal", "Abbesinier", "Bondkatt", "Blandras", "Okänd ras"];


export const getCatBirthYears = [
    2020,
    2019,
    2018,
    2017,
    2016,
    2015,
    2014,
    2013,
    2012,
    2011,
    2010,
    2009,
    2008,
    2007,
    2006,
    2005,
    2004,
    2003,
    2002,
    2001,
    2000];
