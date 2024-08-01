document.addEventListener('DOMContentLoaded', () => {

    populateAvailableTablesDropdown();


    document.getElementById('reservationForm').addEventListener('submit', async (event) => {
        event.preventDefault();

        const firstName = document.getElementById('firstName').value;
        const lastName = document.getElementById('lastName').value;
        const phoneNumber = document.getElementById('phoneNumber').value;
        const tableId = document.getElementById('tableId').value;
        const date = new Date(document.getElementById('date').value);

 
        const today = new Date();
        today.setHours(0, 0, 0, 0); 

        if (date < today) {
            alert('Data musi być ustawiona na przyszłą');
            return;
        }

        try {
         
            let userResponse = await fetch('http://localhost:5000/api/user', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ firstName, lastName, phoneNumber }),
            });

            if (!userResponse.ok) {
                throw new Error('Nie udało się stworzyć użytkownika');
            }

            const user = await userResponse.json();

       
            const reservationResponse = await fetch('http://localhost:5000/api/reservation', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    userId: user.id,
                    tableId,
                    date: date.toISOString(),
                    firstName,
                    lastName,
                    phoneNumber
                }),
            });

            if (reservationResponse.ok) {
                const reservation = await reservationResponse.json();
                alert('Rezerwacja pomyślna');
                console.log(reservation);
              
            } else if (reservationResponse.status === 409) {
                const errorResponse = await reservationResponse.json();
                alert(`Error: ${errorResponse.message}`);
            } else {
                throw new Error('Nie udało się zarezerwować');
            }
        } catch (error) {
            console.error('Error:', error);
            alert('An error occurred. Please try again.');
        }
    });

 
    document.getElementById('availabilityForm').addEventListener('submit', async (event) => {
        event.preventDefault();

        const date = document.getElementById('availabilityDate').value;

        try {
            const response = await fetch(`http://localhost:5000/api/reservation/available-tables?date=${encodeURIComponent(date)}`);
            if (!response.ok) {
                throw new Error('Failed to fetch available tables');
            }
            const tables = await response.json();
            displayAvailableTables(tables);
        } catch (error) {
            console.error('Error:', error);
            alert('An error occurred while fetching available tables.');
        }
    });
});

async function populateAvailableTablesDropdown() {
    try {
        const response = await fetch('http://localhost:5000/api/reservation/available-tables?date=2024-07-31');
        if (!response.ok) {
            throw new Error('Failed to fetch available tables for default date');
        }
        const tables = await response.json();
        const tableSelect = document.getElementById('tableId');
        tableSelect.innerHTML = ''; 

        tables.forEach(table => {
            const option = document.createElement('option');
            option.value = table.id;
            option.textContent = `Stolik numer: ${table.number} (Liczba krzeseł: ${table.seats})`;
            tableSelect.appendChild(option);
        });
    } catch (error) {
        console.error('Error:', error);
        alert('An error occurred while populating the tables dropdown.');
    }
}

function displayAvailableTables(tables) {
    const list = document.getElementById('availableTablesList');
    list.innerHTML = '';

    if (tables.length === 0) {
        list.innerHTML = '<li>Nie ma dostępnych stolików na podana datę.</li>';
        return;
    }

    tables.forEach(table => {
        const listItem = document.createElement('li');
        listItem.textContent = `Stolik numer: ${table.number} (Liczba krzeseł: ${table.seats})`;
        list.appendChild(listItem);
    });
}
