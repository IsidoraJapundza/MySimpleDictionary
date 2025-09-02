# MySimpleDictionary

## Implementirana je generička klasa  'MySimpleDictionary<TKey, TValue>' u C#, koja pruža osnovne funkcionalnosti ugrađenog 'Dictionary<TKey, TValue>'

### Klasa podržava:
- Osnovne konstruktore za kreiranje  
- Dodavanje novih elemenata i mogućnost da se iz rečnika dohvati vrednost na osnovu ključa  
- Proveru postojanja ključeva i vrednosti  
- Uklanjanje pojedinačnih elemenata, kao i brisanje celog sadržaja  
- Pristup određenom elementu (indexer)  
- Mogućnost iteriranja kroz elemente rečnika  
- Pregled broja elemenata i listu svih ključeva i vrednosti

## Benchmark testovi
Dodata su i jednostavna poređenja performansi između 'MySimpleDictionary' i ugrađenog 'Dictionary<TKey, TValue>' pomoću klase 'Stopwatch'.  

Testovi mere:
- vreme dodavanja velikog broja elemenata ('Add')  
- vreme pristupa elementima ('this[key]')  

Rezultati pokazuju da 'MySimpleDictionary' radi korektno, ali je sporiji od ugrađenog 'Dictionary' rešenja.
