---
type: "Anwenderwissen"
title: "HmCalculator: Formeln und Nutzungshinweise"
description: "Anwenderhinweise zu HmCalculator: Formeln und Nutzungshinweise."
tags: ["klims", "anwender", "proben"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/proben/proben-grundlagen-probenformeln-md-5280b029"
---
# HmCalculator: Formeln und Nutzungshinweise

# HmCalculator: Formeln und Nutzungshinweise

siehe auch hier: <externer Link entfernt> 
## Übersicht
Der **HmCalculator** bietet verschiedene mathematische, logische und stringbezogene Funktionen, die in dynamischen Parameterformeln genutzt werden können. Diese Dokumentation beschreibt die verfügbaren Formeln detailliert und zeigt praktische Beispiele.

---

## 1. **Mathematische Grundfunktionen**

### 1.1 `POW(basis, exponent)`
- **Beschreibung**: Berechnung der Potenz durch Anheben einer Basis zu einem Exponenten.
- **Beispiel**:
  ```text
  Eingabe: POW(2, 3)
  Ausgabe: 8

  Eingabe: POW(10, 0)
  Ausgabe: 1
  ```

---

### 1.2 `ILOG10(wert)`
- **Beschreibung**: Berechnet den ganzen Teil des Logarithmus zur Basis 10 eines Wertes.
- **Beispiel**:
  ```text
  Eingabe: ILOG10(100)
  Ausgabe: 2

  Eingabe: ILOG10(1000)
  Ausgabe: 3
  ```

---

### 1.3 `IMUL(a, b)`
- **Beschreibung**: Multipliziert zwei Werte.
- **Beispiel**:
  ```text
  Eingabe: IMUL(5, 6)
  Ausgabe: 30
  ```

---

### 1.4 `IDIV(a, b)`
- **Beschreibung**: Führt eine ganzzahlige Division zwischen zwei Werten durch.
- **Beispiel**:
  ```text
  Eingabe: IDIV(10, 3)
  Ausgabe: 3

  Eingabe: IDIV(9, 2)
  Ausgabe: 4
  ```

---

### 1.5 `RELAXEDSUM(wert1, wert2, ...)`
- **Beschreibung**: Berechnet die Summe der Werte und ignoriert nicht-numerische Eingaben.
- **Beispiele**:
  ```text
  Eingabe: RELAXEDSUM(1, 2, 'abc', 4)
  Ausgabe: 7

  Eingabe: RELAXEDSUM('1', '2,1', '<10', 'abc', 4)
  Ausgabe: 7,1

  Eingabe: RELAXEDSUM('')
  Ausgabe: 0

  Eingabe: RELAXEDSUM('<0,1', '1')
  Ausgabe: 1
  ```

---

### 1.6 `GEOMETRICMEAN(wert1, wert2, ...)`
- **Beschreibung**: Berechnet das geometrische Mittel der angegebenen Werte.
- **Beispiele**:
  ```text
  Eingabe: GEOMETRICMEAN(4, 1, 16)
  Ausgabe: 4

  Eingabe: GEOMETRICMEAN(3, 5, 15)
  Ausgabe: 6,71 (gerundet)

  Eingabe: GEOMETRICMEAN('<10', '10', '20')
  Ausgabe: 12,6 (gerundet)
  ```

---

## 2. **String-Operationen**

### 2.1 `REGEXREPLACE(eingabe, muster, ersatz)`
- **Beschreibung**: Ersetzt Teile eines Strings, die einem regulären Ausdrucks-Muster entsprechen, durch einen Ersatztext.
- **Beispiele**:
  ```text
  Eingabe: REGEXREPLACE('123abc456', '\d', '#')
  Ausgabe: '###abc###'

  Eingabe: REGEXREPLACE('D-12345', '^D-', '')
  Ausgabe: '12345'

  Eingabe: REGEXREPLACE('<10', '<', '')
  Ausgabe: '10'
  ```

---

### 2.2 `LEFT(string, länge)`
- **Beschreibung**: Gibt die ersten Zeichen eines Strings bis zur angegebenen Länge zurück.
- **Beispiele**:
  ```text
  Eingabe: LEFT('Hallo Welt', 5)
  Ausgabe: 'Hallo'

  Eingabe: LEFT('12345', 3)
  Ausgabe: '123'
  
  Eingabe: LEFT('<10', 1)
  Ausgabe: '<'
  ```

---

### 2.3 `JOIN(string1, string2, ...)`
- **Beschreibung**: Verknüpft mehrere Strings.
- **Beispiele**:
  ```text
  Eingabe: JOIN('Hallo', ' ', 'Welt')
  Ausgabe: 'Hallo Welt'

  Eingabe: JOIN('', '1', '2', '3')
  Ausgabe: '123'

  Eingabe: JOIN('-', '2023', '10', '15')
  Ausgabe: '2023-10-15'
  ```

---

### 2.4 `FORMATDATE(datum, formatString)`
- **Beschreibung**: Formatiert ein Datums-String entsprechend der angegebenen Formatzeichenfolge.
- **Beispiele**:
  ```text
  Eingabe: FORMATDATE('2023-10-15', 'MM/dd/yyyy')
  Ausgabe: '10/15/2023'

  Eingabe: FORMATDATE('13.01.2021', 'yyyy-MM')
  Ausgabe: '2021-01'

  Eingabe: FORMATDATE('13.01.2021', 'dd-yyyy-MM')
  Ausgabe: '13-2021-01'
  ```

---

### 2.5 `LASTLISTELEMENT(liste, delimiter)`
- **Beschreibung**: Gibt das letzte Element einer durch ein Trennzeichen getrennten Liste zurück.
- **Beispiele**:
  ```text
  Eingabe: LASTLISTELEMENT('1;2;3', ';')
  Ausgabe: '3'

  Eingabe: LASTLISTELEMENT('Apfel; Birne; Banane', ';')
  Ausgabe: 'Banane'

  Eingabe: LASTLISTELEMENT('1,2,3', ',')
  Ausgabe: '3'
  ```

---

## 3. **Logische Funktionen**

### 3.1 `IIF(bedingung, trueResult, falseResult)`
- **Beschreibung**: Führt eine Bedingung aus und gibt je nach Ergebnis einen von zwei Werten aus.
- **Beispiele**:
  ```text
  Eingabe: IIF(5 > 3, 'Ja', 'Nein')
  Ausgabe: 'Ja'

  Eingabe: IIF(1 = 2, 'Richtig', 'Falsch')
  Ausgabe: 'Falsch'

  Eingabe: IIF(ISNUMERIC('123'), 'Zahl', 'Nicht Zahl')
  Ausgabe: 'Zahl'
  ```

---

### 3.2 `CASE(value, case1, result1, ..., elseResult)`
- **Beschreibung**: Implementiert eine "Switch Case"-Logik mit einer Standardaktion.
- **Beispiele**:
  ```text
  Eingabe: CASE(2, 1, 'Eins', 2, 'Zwei', 'Andere')
  Ausgabe: 'Zwei'

  Eingabe: CASE('a', 'a', '1. Fall', 'b', '2. Fall', 'Kein Treffer')
  Ausgabe: '1. Fall'

  Eingabe: CASE(4, 1, 'Eins', 2, 'Zwei', 'Standardwert')
  Ausgabe: 'Standardwert'
  ```

---

### 3.3 `IFNULL(value, defaultValue)`
- **Beschreibung**: Gibt den Standardwert zurück, falls der Eingabewert `null` ist.
- **Beispiele**:
  ```text
  Eingabe: IFNULL(null, 'Standard')
  Ausgabe: 'Standard'

  Eingabe: IFNULL('Wert', 'Standard')
  Ausgabe: 'Wert'
  ```

---

### 3.4 `FIRSTNOTNULL(value1, value2, ...)`
- **Beschreibung**: Gibt den ersten Wert in der Liste zurück, der nicht `null` ist.
- **Beispiele**:
  ```text
  Eingabe: FIRSTNOTNULL(null, null, 'Hallo')
  Ausgabe: 'Hallo'

  Eingabe: FIRSTNOTNULL('', 'Fallback', 'Wert')
  Ausgabe: 'Fallback'
  ```

---

### 3.5 `STAIRCASE(wert, limit1, result1, limit2, result2, ..., defaultResult)`
- **Beschreibung**: Implementiert eine gestaffelte Bedingungslogik.
- **Beispiele**:
  ```text
  Eingabe: STAIRCASE(15, 10, 'Niedrig', 20, 'Mittel', 'Hoch')
  Ausgabe: 'Mittel'

  Eingabe: STAIRCASE(25, 10, 'Niedrig', 20, 'Mittel', 'Hoch')
  Ausgabe: 'Hoch'
  ```

---

### 3.6 `IFNOTNUMERIC(value, defaultValue)`
- **Beschreibung**: Gibt den Standardwert zurück, falls der Eingabewert nicht numerisch ist.
- **Beispiele**:
  ```text
  Eingabe: IFNOTNUMERIC('a', 0)
  Ausgabe: 0

  Eingabe: IFNOTNUMERIC('123', 0)
  Ausgabe: 123
  ```

---
