---
type: "Anwenderwissen"
title: "Rollen & Berechtigungen"
description: "Anwenderhinweise zu Rollen & Berechtigungen."
tags: ["klims", "anwender", "administration"]
timestamp: "2026-07-23T11:18:26Z"
provenance: "KLims-Anwenderwiki"
confidence: "wiki-only"
audience: "end-user"
resource: "okf://klims/anwenderwissen/administration/inhaltsverzeichnis-rollen-berechtigungen-md-3568f0b0"
---
# Rollen & Berechtigungen

**Zugriffsrechte IT - K-Lims-„Rollen“**

| **Rollen** | **technische Berechtigungen (für Benutzeranlage)** |
| -------- | -------- |
| **Buchhaltung³** | LK.Intern <br>LK.Rechnungserstellung <br>Mad/Kunden/DeletePutPost <br>Mad/Fahrtkostenpauschalen/Get |
| **Mibi** | LK.Intern |
| **ELISA** | LK.Intern |
| **PCR** | LK.Intern |
| **Virologie**| LK.Intern |
| **Sequenzierung** | LK.Intern |
| **Chemie** | LK.Intern |
| **Instrumentelle Analytik** | LK.Intern |
| **Wareneingang** | LK.Intern <br>Mad/Kunden/DeletePutPost |
| **Wasser** | LK.Intern <br>Mad/Kunden/DeletePutPost <br>Mad/Kundenimporte/DeletePutPost <br>Mad/Translations/DeletePutPost |
| **Histologie** | LK.Intern <br>Mad/Kunden/DeletePutPost |
| **Tourenplanung** | LK.Intern <br>Sys/Singletons/Get <br>Sys/Singletons/Put <br>Mad/Kunden/DeletePutPost <br>Mad/Fahrtkostenpauschalen/Get |
| **Probenannahme** | LK.Intern |
| **Sachverständige** | LK.Intern <br>Mad/Kunden/DeletePutPost <br>Mad/Kundenimporte/DeletePutPost <br>Mad/Translations/DeletePutPost |
| **Prüfleitung** | LK.Intern <br>LK.FremdlaborAuftragErzeugung <br>LK.Mikrobiologie <br>LK.Prüfleiter <br>Mad/Auswertungen/DeletePutPost <br>Mad/Kundenexporte/DeletePutPost <br>Mad/Parameter/DeletePutPost <br>Mad/Pruefberichtinfos <br>Sys/Singletons/Get <br>Sys/Singletons/Put <br>Mad/Translations/DeletePutPost <br>Mad/Prueflisten/DeletePutPost |
| **Sekretariat** | LK.Intern <br>Mad/Kunden/DeletePutPost <br>Mad/Kundenexporte/DeletePutPost |
| **Angebot** | LK.Intern <br>Mad/Fahrtkostenpauschalen/DeletePutPost |

**Zugriffsrechte IT – erweiterte Berechtigungen**
| **Berechtigung** | **technische Berechtigungen (für Benutzeranlage)** |
| -------- | -------- |
| Fremdlaboraufträge erzeugen | LK.FremdlaborAuftragErzeugung |
| Aldi BakeOff Auftragsplanung | LK.AldiBakeOffPlanung |
| Probennannahme Importer | A.Probenannahme |
| Aldi Süd (ISQM) Rechnungspostenexport | Kunden/AldiSued/Rechnungsposten |
| Auftragsplanung | A.Tourenplanung |
| Auswertung "Parameterbearbeitung" | LK.Auswertungen.Parameterbearbeitung|
| Sachverständige erweitert (Farbschema, <br>Stammdatenpflege Aldi Nord, EDEKA-Prüfbericht <br>Berns übermitteln, Probenkontrolle, …) | A.Sachverstaendige|
| Aldi BakeOff Berns (Ergebniserfassung) | LK.ProbenahmeBerns |
| Stammdatenpflege allgemein| LK.Stammdatenpfleger |
| Kundenstammdaten und Kundeninformationen pflegen | Mad/Kunden/DeletePutPost |
| Buchhaltung allgemein³ | LK.Rechnungserstellung |
| Farbschemen pflegen | Mad/Farbschemas |
| QM Auswertungen | LK.QM |
| Aldi Nord Stammdatenpflege | LK.AldiNord |
| Prüfleitung allgemein | LK.Prüfleiter |
| Probenkontrolle | LK.Probenkontrolle |
| Superuser³ | LK.Superuser |
| Mikrobiologie allgemein | LK.Mikrobiologie |
| Histologie allgemein | LK.Histologie |
| Auswertungsvorlagen pflegen | Mad/Auswertungen/DeletePutPost |
| Livecheckkonfigurationen pflegen | Mad/Livechecks |
| Basisparameter pflegen | Mad/Parameter/DeletePutPost |
| Probenzerkleinerung allgemein | LK.Probenzerkleinerung |
| Kaufland Importer | LK.Kaufland |
| Klims Konfiguration anzeigen | Sys/Singletons/Get |
| Generischer Excel-Kundenimport | Mad/Kundenimporte/DeletePutPost |
| Produktklassen editieren | Mad/Produktklassifizierung/Produktklassen/Put |
| Proben-Logs anzeigen | LK.Logs |
| PAV Auftraggeber pflegen | Mad/Pav/Auftraggebers/DeletePutPost |
| Stammdatenpflege Beauftragung <br>(Analysenzuordnungen, Analysen, Methoden, ...) | Mad/Beauftragungen/DeletePutPost |
| Stammdatenpflege Prüfberichtübersetzungen | Mad/Translations/DeletePutPost |
| Klims Konfiguration pflegen | Sys/Singletons/Put |
| Stammdatenpflege Prüflisten | Mad/Prueflisten/DeletePutPost |
| Auftragsvorlagen pflegen | Opd/Probenanlage/Auftragsvorlagen/DeletePutPost |
| Lidl allgemein | LK.Lidl |
| Kundenexporte pflegen | Mad/Kundenexporte/DeletePutPost |
| Prüfberichtinfos pflegen | Mad/Pruefberichtinfos |
| Produktattribute pflegen | Mad/Produktklassifizierung/DeletePutPost |
| Produktklassen pflegen | Mad/Produktklassifizierung/Produktklassen/DeletePutPost |
| Stammdatenpflege <br>Auftragswiederverwendung | Mad/Probenanlage/Auftragsvalidierungen/DeletePutPost |
| Fahrtkostenpauschalen anzeigen | Mad/Fahrtkostenpauschalen/Get |
| Fahrtkostenpauschalen pflegen | Mad/Fahrtkostenpauschalen/DeletePutPost |
| Zurücksetzen von Messaufträgen | Opd/Geraete/Reset |

**³diese Berechtigungen sind durch die Geschäftsführung freizugeben!**
