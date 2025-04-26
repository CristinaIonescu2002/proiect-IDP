### Proiect-IDP
#### Cojocaru Andrei Radu 341C4 + Ionescu Cristina 341C3

#### Descriere
Platforma noastra este dedicata comunitatii artistice si are ca scop facilitarea accesului la referinte vizuale utile in procesul creativ. Utilizatorii pot incarca imagini de referinta, pot asocia lucrarile proprii cu aceste referinte, si pot naviga continutul prin filtre si topicuri de interes. Platforma ofera si o componenta sociala, prin posibilitatea de a urmarii topicuri de interes, de a aprecia sau comenta lucrarile postate.

#### Accesari
- front: ``http://localhost:3000/``
- swagger prin kong: ``http://localhost:8000/swagger/index.html``
- portainer: ``http://localhost:9000``
- prometheus: ``http://localhost:9090``
- grafana: ``http://localhost:3001``

#### Docker
- init swarm ``docker swarm init``
- deploy stack ``docker stack deploy -c docker-compose.swarm.yml art-platform``
- verificari ``docker stack services art-platform`` si ``docker stack ps art-platform``

-- trebuie rulat intr-un docker swarm

-- tre sa facem asta

Aplicația voastră va trebui să ruleze corect pe un cluster format dintr-un manager și doi workers, și
va trebui să includă și componente de orchestrare. Mai precis, în configurația de stivă trebuie neapărat
să includeți tag-uri de deployment (este la latitudinea voastră cum gândiți deployment-ul, dar va trebui
să aveți aceste elemente prezente).
Microserviciile din aplicația voastră vor trebui separate logic prin intermediul rețelelor. Nu se vor
accepta proiecte în care toate serviciile fac parte din aceeași rețea.


