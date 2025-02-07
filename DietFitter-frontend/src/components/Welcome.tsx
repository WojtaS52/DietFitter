// import { useState } from "react";

export default function Welcome() {

    return (
        <div className="welcome">
            <h1>Witaj w aplikacji DietFitter</h1>
            <p>Witaj w aplikacji DietFitter, która pomoże Ci śledzić twoje parametry zdrowotne (jak np. poziom magnezu, cukru, bądź cholesterolu). Pozatym moja aplikacja posiada również autorski algorytm doboru diety ze względu na problemy metaboliczne. Pomaga dobrać on odpowiednie składniki, które warto dostarczyć w przypadku niedoborów. Doradza również w drugą strone w przyapdkach takich jak np. nadwaga, albo insulinooporność.</p>
            <p>Na górze masz pasek nawigacyjny. Wspomaga on łatwiejsze porusznaie się po interfejsie. Jeśli chcesz skorzystac z algorytmu doboru diety klikniej "Algorytm", jeśli śledzić swoje parametry, wejdź na "Profil". Oczywiście dostęp do ustawień też jest.</p>
        </div>
    );

}