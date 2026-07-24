---
type: "Anwenderwissen"
title: "Beauftragung"
description: "Anwenderhinweise zu Beauftragung."
tags: ["klims", "anwender", "stammdaten"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/stammdaten/stammdaten-beauftragung-md-922d2e15"
---
# Beauftragung

# **Beauftragung**
### **Analysen:**
_neue Analysen erstellen_
- unter Analysen neue Analysen oder Analysenpakete definieren. 
- Analysenbezeichnung wird allgemein verständlich gehalten, Analysennamen werden nicht mit Abteilungskürzel versehen. 
- Analysenpaketnamen meist mit "chemische Untersuchung..." oder ähnlichen Floskeln begonnen. Sonderfall sind "Paketparameter". Über diese ist es möglich eine Analysengruppe z.B. Konservierungstoffe mit wenig Aufwand zu beauftragen. Eine digitale Rückübermittlung der Ergebnisse mehrere Parameter (Sorbinsäure, Benzoesäure, phB-Ester) erfolgt allerdings dann nur auf die beauftragte Analyse Konservierungsstoffe. Soll je Verbindung/Parameter das Ergebnis zurück übermittelt werden, sind die Analysen auch einzeln zu beauftragen.
- Attribute ignorieren, wird ausgewählt, wenn der Kunde keine Rückübermittlung will und er Analysenparameter nur im PB auftauchen soll.
- detaillierter Leitfaden siehe hier: Y:\BUL\Sachverstaendige\Persoenliche Ordner\P.Breunig\KLims Checkliste neue Analyse

_Für Abrechnungsparameter keine Analysen und Analysenzuordnungen erstellen, sondern den Analysen und Analysenpaketen die Basisparameter direkt zu ordnen (z.B. Mibi Grundpreis, Enzymatikvorbereitung)._

## **Eigenschaften:**

|**Name**|  **Bedeutung**|
|--|--|
| Name | Dieser Name wird auf der öffentlichen Analysenliste (<externer Link entfernt>) angezeigt und durch den Kunden beauftragt. Diesem Namen werden in der Analysenzuordnung die Basisparameter zugeordnet. Der Paketname laut in der Regel z.B. "Chemische Untersuchung ..." oder "Untersuchung auf". Der Parametername bezeichnet den zu untersuchenden Parameter. Infos zu Methoden oder zu Matrices werden in der Regel nicht gegeben (Ausnahmen siehe z.B. Trockenmasse). |
| Typ | Der Typ unterscheidet zwischen Parameter und Paket. |
| Sortierung | Die Sortierung legt die Reihenfolge der Pakete im HMlims und damit auch im Prüfbericht fest. Die Sortierung wird mit einer fünfstelligen Zahl (z.B. 00100 ggf. mit /1, /2..., näheres siehe Ende der Tabelle) definiert. Untersuchungen, die in der gleichen Unterprobe angelegt werden sollen, sind mit der gleichen fünfstelligen Nummer zu versehen. Ihre Reihenfolge in der Unterprobe wird mit /1 usw. definiert (siehe u.a. Histo oder Schwermetalle). |
| Interner Name| Der interne Name ist nur bei Paketen relevant. Der interne Name ist der entsprechende Name des Pakets im HMlims und wird von dort exakt übernommen. Dadurch wird sichergestellt, dass der Preis entsprechend hinterlegt wird. Alternativ kann der Name aus Klims in der Preisliste direkt hinterlegt werden (z.B. Chemische Untersuchung auf Big 7), dann ist das Feld leer. In letzterem Fall ist ein entsprechender Kommentar einzufügen. |
|  Ausgabename| Der Ausgabename wird auf dem Prüfbericht gedruckt. Falls dieses Feld nicht gefüllt wird, wird der "Name" im Prüfbericht ausgegeben. |
|  Intern?| Interne Parameter sind nur zur internen Verwendung gedacht und werden nicht auf der öffentlichen Analysenliste (<externer Link entfernt>) angezeigt. |
|  Technisch?| Technische Parameter gibt es, um kompliziertere Zuordnungen mit Zwischenschritten zu ermöglichen (z.B. Knochenteilchen Histologie); sie werden weder auf der öffentlichen Analysenliste noch in den Auswahllisten in der Auftragserfassung angezeigt. |
| Alias |Äquivalente Bezeichnungen zum Namen der Analyse, die im Auftrag verwendet werden dürfen (z.B. GKZ statt Gesamtkeimzahl) |
| Beschreibung | Eine offizielle Beschreibung der Analyse, die in der öffentlichen Analysenliste angezeigt wird. |
| Kommentar | Der Kommentar ist intern und wird nicht in der öffentlichen Analysenliste angezeigt. |

### **Reihenfolge Paketsortierung:**
_01000 Sensorik (Verkostung, Spezifikationsabgleich...)
01400 Kennzeichnung
01500 Sensorik (Gewichte, Präparation,...)
02000 Mibi
03000 Chemie (Big...)
03100 Chemie (Vollanalyse...)
03200 Chemie (Chemische Untersuchungen...)
03300 Chemie (Fettsäuren...)
03500 Chemie (weitere...)
04000 Zusatzstoffe
05000 Histo
06000 Tierart, Geschlechtsbestimmung, GMO, ZNS, Allergen
07000 Biozide: Pflanzenschutzmittel, ...
08000 Tierarzneimittel
09000 Kontaminanten (Metalle Paket bedingt bei Chemie 03000)
10000 Mykotoxine
11000 Vitamine
12000 Sonstiges
15000 Sachverständige
16000 Nährmedien
20000 Wasser_
_Z... Aldi Bake off_

_Reiehfolge Parameter: hier ist die Nummerierung entweder an die Paketnummerierung angelehnt oder willkürlich (vereinheitlichen pebr 22/10/2021) ?_

### **Analysenzuordnungen erstellen:**
- **unter Analysenzuordnung werden den im KLims erstellten** 
a) _Analysenparameter die Basisparameter (CAVE: Unterschiede zw. Basisparameter und Parameter im HMlims Paket) aus dem HMLims s**owie, soweit aufgrund der Preisstruktur notwendig, die Grundpreise (hier Präzisierung in Hilfe noch notwendig, Mibi IA...)** zugeordnet. Für häufig verwendete Basisparameter (z.B. Fett) gibt es Platzhalteranalysen wie C Fett...
b) Die Analysenpakete werden durch die Zuordnung von Analysen erstellt. Hier können Analysen nachgelegt und als optional definiert werden.
Nachlegen bedeutet, dass die Analyse, wenn das Analysenpaket erkannt wird, automatisch in das Paket gezogen werden, ohne dass eine Beauftragung erfolgt war. Nachlegen ist nur bei Paketanalysen möglich.
Optional bedeutet, dass die Analyse im Analysenpaket vorkommen kann aber nicht muss (z.B. Zucker oder NPN in einer Vollanalyse)
Analysenpaket werden nach Anzahl nicht optionaler Analysenparameter ausgewählt. Das Analysenpaket mit der größten Übereinstimmung an nicht optionaler Analysenparameter wird verwendet. 
Der durch die Verwendung der Produktklasse wird der Einsatzbereich der jeweiligen Analysenparameter und -pakete definiert._
- _Der durch die Verwendung der Methode wird die zu verwendende Methode (z.B. nasschemisch) festgelegt. Derzeit ist die Verwendung insbesondere bei VIL von Mibi Methoden üblich, um die entsprechenden kundenspezifischen Verdünnungstufen sicherzustellen (Dazu sind Analysen in unterschiedlichen Verdünnungen anzulegen. Das ist im Klims derzeit noch nicht vorhanden.) Werden in der Paketzuordnung Mehtoden verwendet, so ist die Methode im Auftrag auf Positionsebene anzugeben (siehe Weichmacher pebr).
Kommentar: interne Information_ 

### **Logik der Analysenzuordnung zu Kundenauftragsparameter:**
- im Auftrag sind folgende Angaben verpflichtend: Analyse und Produktklasse, optional sind untersuchter Anteil und Methode 
- Zuerst wird die im Kundenauftrag angegeben Analyse in der Analysenzuordnung gesucht, dann die gefundenen Analysenzuordnungen auf mögliche Übereinstimmungen bei den Angaben Produktklasse, untersuchter Anteil und Methode geprüft. Für jede dieser drei Informationen werden Gewichtungspunkte vergeben: bei der Produktklasse 0-1 Punkt je nach Übereinstimmung, bei untersuchter Anteil 1 Punkt, wenn Übereinstimmung vorhanden, und bei Methode 1 Punkt, wenn Übereinstimmung vorhanden. Die Analysenzuordnung mit der höchsten Übereinstimmung, dh. mit der höchsten Punktzahl wird für den Kundenauftrag verwendet. Besteht Punktgleichheit bei zwei Zuordnungen, wird eine Übereinstimmung des untersuchten Anteils bevorzugt und dann eine Übereinstimmung der Methode.
- Hinweis/Beispiel: Nitrat in Analyse (Paket) "Chemische Untersuchung auf Nitrit/Nitrat" auf optional gesetzt, da sonst Paket nicht erkannt wird bei Beauftragung mit Nitrat (als NaNO3), da Parameter der obligatorischen Analyse über die obligatorischen Analyse beauftragt werden muss und beim Paketbau nicht erkannt wird, wenn diese Parameter der obligatorischen Analyse über eine andere optionale Analyse angelegt werden. JueGr/PeBr 23.04.20

### **Logik der Einstufung von Analysenpaketen: intern und/oder technisch:**
- nachzulegen ist durch Unterstreichung der Analyse in der Analysenzuordnung kenntlich gemacht
- optional ist durch kursive Schrift der Analyse in der Analysenzuordnung kenntlich gemacht

**Alle Analysen optional und nachzulegen --> nicht intern und nicht technisch:**
- Szenario 1 aktuell:
- Präzise Beauftragung von Analysen (z.B. Sorbinsäure, Benzoesäure, PHB-Ester) über Schnittstelle mit Attributen je Analyse möglich und dann automatisches Nachlegen der restlichen nicht beauftragten
- Kundenfreundlich, da meistens Preis für Paket, d.h. der Kunde bekommt mehr, d.h. alle möglichen, Ergebnisse fürs gleich Geld
- Vereinfachte Beauftragung über Analysenpaket (z.B. Untersuchung auf Konservierungsstoffe) möglich, da hier alle enthalten Analysen nachgelegt werden
- Schwachpunkt, wenn ein Kunde nur z.B. einen Konservierungsstoff untersucht haben will, werden alle anderen automatisch nachgelegt. 
- Szenario 2: Analysen optional und Paketparameter. Live checks für Kunden? Hier dann Nacharbeiten notwendig, bisher nur Kugler. Betroffene Paket siehe Übersicht pebr: Y:\BUL\Sachverstaendige\Persoenliche Ordner\P.Breunig\ Klims Uebersicht bez. Analysenpaketen optional nachzulegen, Konservierungstoffproblematik.xlsx

**Alle Analysen optional --> intern und technisch (unbedingt, da hier ja kein Analyse beauftragt und angelegt wird):**
- Verpflichtende Analysen enthalten
- ein verpflichtender Analysen vorhanden --> intern und technisch
- nur ein verpflichtender Analysen
- ein verpflichtender Analysen vorhanden und weitere optionale vorhanden
- ein verpflichtender Analysen vorhanden und weitere optionale/nachzulegende vorhanden
- mehrere verpflichtender Analysen vorhanden --> Vorgehen noch zu prüfen
- ggf. intern aber nicht technisch, 
- wenn vereinfachte Beauftragung erwünscht ist (z.B. Käsezusammensetzung) ?

### **Tips & Tricks:**
_~-Parameter (Tilde)_

- dienen als Attributträger, wenn eigentlicher Parameter nicht angelegt werden kann (Fleischanteil) oder wenn es sich um Wenn-Dann-Parameter (Berns EDEKA: kondensierte Phosphate) handelt
- der Name des ~-Parameters muss dem Ausgabenamen im Prüfbericht entsprechen. Dann wird diesem nachgelegten Parameter die Attribute des ~-Paremeters zugeordnet.
- der Tildenparameter wird immer dann erzeugt, wenn einer Analyse kein Basisparameter / Analyse zugeordnet ist (z.B. ESBL)

### **DLG-Beauftragung:**
_Beauftragung über "Sensorik nach DLG-Schema" oder "DLG-Qualitätszahl"
primär für REWE ersonnen._

### **DLG-Qualitätszahl:**
- ist die Basis und zieht alle benötigten Parameter in KOmbination mit der richtigen Produktklasse nach
- Die Qualitätszahl hat selbst keine spezifisch Produktklassenzuordnung.
- Ergebnisparameter ist der Basisparameter S DLG Qualitätszahl, die dazu notwendigen S DLG Summe der Bewertung bzw. Faktoren sind auch zugeordnet
- folgende Analysen werden nachgezogen: DLG-Prüfschema, DLG-Parameter, DLG-Abwertung

### **DLG-Prüfschema:**
- gibt es als Analyse **mit spezifischer Produktklassenzuordnung;** damit wird der richtige Name des Prüfschemas im Lims definiert
- in Feld "fester Wert" wird der gewünschte Name hinterlegt
- gibt es als Analyse ohne Produktklassenzuordnung zum manuellen Eintrag für weniger häufige Schemen.

### **DLG-Parameter und DLG-Parameter "Prüfschema":**
- gibt es als Analyse **mit spezifischer Produktklassenzuordnung;** diese ist auf die Analyse DLG-Parameter "Prüfschema" und ggf. DLG-Zubereitung gemappt. Die Analyse DLG-Parameter "Prüfschema" dient der Übersichtlichkeit, da nicht alle Basisparameter der jeweiligen Produktklasse zugeordnet werden müssen.
- die Analyse DLG-Parameter "Prüfschema" definiert die in den jeweiligen Prüfschemen vorhandenen Parameter und die spezifischen Faktoren .
- in Feld "fester Wert" wird der gewünschte Faktor hinterlegt. "Fester Wert" kann nur hinterlegt werden, wenn es sich um eine Hilfsparameter und nicht um einen Analysenparameter handelt.

### **DLG-Abwertung:**
- gibt es als Analyse ohne Produktklassenzuordnung
- sind die Basisparameter S Sensorikabwertung und S Sensorikabwertung 1 zugeordnet

### Info
- DLG-Prüfschema  S DLG Prüfschema ["Teigwaren (trocken)"] mit PK mappen
- DLG-Parameter Teigwaren, trocken auf Basisparameter gemäß Paket ohne PK mappen
- DLG-Parameter auf DLG-Parameter Teigwaren, trocken auf Basisparameter mit PK mappen, ggf. mehrmals
oder 
- DLG-Parameter auf Basisparameter gemäß Paket ohne PK mappen |Standard|Lebensmittel|Fisch|Fischerzeugnisse| Räucherfisch|ganze, halbe Fische

### **Histo-Bestellung:**
**Ziel:** 
- Histopakete müssen in eine Unterprobe (Sortierung beachten /1…)
- Histo Standard wird grundsätzlich mitangelegt-

**Parameter:**
- Zuordnung Klims-Analyse zu Basisparameter 
- Hier intern, nicht technisch
- z.B. H wiederverarbeitetes Brät --> H verdichtete Strukturen

**Paket-Parameter:**
- Für Kunden zum Bestellen
- Öffentlich, nicht technisch
- Keine Basisparamteer zugeordnet, sondern Analysen (Parameter, Pakete: hier Histo Standard, da dies grundsätzlich mitangelegt werden soll)
- Histo Standard in Paket-Parameter auf Attribute ignorieren setzen
- Z.B. Histologie Rework (Charvat)--> H wiederverarbeitetes Brät + Paket Histologie Standard

**Pakete (hier technisch):**
- Wird benötigt, dass Pakete angelegt werden (Standard und Rework in einer UP getrennt)
- Hier intern, technisch (erkennbar an HISTO)
- z.B. HISTO Rework (Charvát) --> H wiederverarbeitetes Brät

**Beauftragung Knochen (z.B. Dr. Berns):**
_folgende Analysen sind notwendig um die Probe korrekt zu erzeugen: Knochenteilchen in Partikel/cm² (Histologie)", "Knochenteilchen (Histologie)", "Knochenteilchen mittels Planimetrie (Histologie)" pebr 29.11.2021_

### **Präparation:**
_fixe Hinterlegung über Produktklasse: wird genutzt, wenn Standardprodukte Wurst nach Leitsätzn wie Bierschinken präpariert werden._

**Dynamische Präparation:**
_wird genutzt, wenn Kunde eine Liste zu präparierenden Anteile beauftragt, die nicht einzeln zurückgemeldet werden müssen. Die einzelnen benötigten Parameter werden automatisch hinterlegt z.B:_

- Berns Dr.: hier werden die Attribute in der .jason schon übermittelt
- Rewe: hier werden die Attribute manuell im Kundenauftrag gepflegt (jetzt über Dialog: hier sind explizit alle zu präparienden Anteile anzugeben; früher: erweiterte Ansicht: Attribute 1.Spalte: "$konfiguration", 2.Spalte: "{Anteile: ["Fleisch", "Brötchen"]}
- Änderung dieser Anteile ist im Nachhinein nicht ohne weiteres im HMlims möglich
- Abrechnugsparameter wird bei Anlage ermittelt und hinterlegt.

**Analysen "präparierter Anteil xy":** 
_wird genutzt, wenn Kunde eine Liste zu präparierenden Anteile beauftragt, die **einzeln** zurückgemeldet werden müssen. Die einzelnen benötigten Parameter werden entweder im Kneißler-wording beauftragt oder im Kundenauftragsprofil gemappt_:

- z.B. bei Stockmeyer über SAP-Schnittstelle, REWE
- Preis gepflegt über S präparierter Anteil * usw.
