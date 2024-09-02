# User Management
API for User Management frontend aplikaciju:
<p>https://github.com/ogi997/user-management-clientapp</p>

## Pregled
<p>
  Serverska aplikacija "User Management" je robustan backend sistem razvije pomocu C# i .NET tehnologija. Aplikacija pruza sve potrebne APIje i usluge za podrsku frontend aplikaciji razvijenoj u Angularu 17, omogucavajuci efikasno upravljanje korisnickim podacima.
</p>

### Kljucne funkcionalnosti
<ol>
  <li>
    JWT (JSON Web Token): Generisanje i verifikacija JWTa za autentifikaciju korisnika.
    <p>Prilikom uspjesne prijave korisnika vracaju se dva JWT tokena (access i refresh)</p>
  </li>
  <li>
    Role-Based Access Control (RBAC): Upravljanje pristupom funkcionalnostima na osnovu korisnickih uloga.
  </li>
  <li>
    Upravljanje korisnicima: APIjevi za upravljanje korisnicima
    <p>
      - Moguce je izvrsiti kreiranje novih korisnika, njihov pregled kao i brisanje. 
    </p>
  </li>
  <li>
    Upravljanje korisnickim ulogama:
    <p>
      - Moguce je izvrsiti kreiranje novih korisnickih uloga, njihov pregled i brisanje.
    </p>
    <p>
      - Moguce je izvrsiti dodjelu korisnickih uloga prema korisniku. Jedan korisnik moze da ima vise korisnickih uloga.
    </p>
  </li>
</ol>

### Tehnicka implementacija
<ol>
  <li><b>Framework:</b> <p>.NET 8 za izgradnju backend aplikacije</p></li>
  <li><b>API:</b><p>RESTful APIjevi za komunikaciju sa frontend aplikacijom.</p></li>
  <li><b>Baza podataka:</b> <p>Entity framework Core: ORM (Object-Relational Mapper) za interakciju sa bazom podataka i upravljanje korisnickim podacima.</p></li>
  <li><b>Upravljanje konfiguracijom:</b>
    <ul>
      <li>AppSettings: 
        <p>
          Konfiguracija aplikacije preko `appsettings.json` za postavke poput konekcijskih stringova i drugih parametara.
        </p>
      </li>
      <li>
        <b>Dependency Injection: 
        </b>
        <p>Koriscenje DI za upravljanje zivotnim ciklusom objekata i servisima u aplikaciji.</p>
      </li>
    </ul>
  </li>
</ol>

### Integracija
<p>
  Serverska aplikacija komunicira sa Angular frontend aplikacijom putem REST APIja. Svi zahtjevi zapodatke obavljaju se kroz APIjeve koje pruza serverska aplikacija.
</p>
<p><b>NAPOMENA:</b> Kroz gore navedeni opis nisu obuhvacene sve mogucnosti aplikacije koje su implementirane. Za vise detalja pogledati izvorni kod (source code) same aplikacije</p>
