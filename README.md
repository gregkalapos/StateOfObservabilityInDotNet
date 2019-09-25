# State of Observability in .NET - Tracing applications in the cloud
## Code samples and slides

This repository contains the sample code of my “State of Observability in .NET - Tracing applications in the cloud” talk from Basta 2019, Mainz, Germany.


Egal ob man Applikationen in der Cloud oder im eigenen Rechenzentrum betreibt, Telemetriedaten aus einem Produktionssystem sind für den Erfolg eines Softwareprodukts ausschlaggebend. Cloud und Microservices machen Telemetrie und Tracing allerdings noch wichtiger: Ein HTTP 5xx im Browser sagt uns nicht, wo genau ein Request schiefgegangen ist; mit Dutzenden von Cloud-Services den Fehler zu finden ist gar nicht so trivial. Zum Glück stellt .NET sehr gute Features bereit, um Tracingsysteme zu bauen – ein Beispiel dafür ist DiagnosticSource. Zusätzlich gibt es auch eine W3C-Initiative für einen Tracingstandard namens Trace Context sowie Open-Source-Initiativen mit .NET-Unterstützung, wie zum Beispiel OpenTelemetry.

Was heißt das für .NET-Entwicklerinnen und -Entwickler, die Applikationen für die Cloud bauen? Diese Session zeigt, wie DiagnosticSource, Distributed Tracing und OpenTracing funktionieren. Das ist aber noch nicht die ganze Geschichte: Die vorher genannten Tools bieten eine Lösung, um Daten zu sammeln und zu korrelieren. Zusätzlich möchte man natürlich diese Tracing- und Telemetriedaten auch visualisieren. Der zweite Teil der Session beschäftigt sich daher mit Observability und Monitoringtools, die die Telemetriedaten auch sammeln, speichern und visualisieren. Vorgestellt werden Application Insights, Jaeger und Elastic APM.
