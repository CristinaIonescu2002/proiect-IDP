### Proiect-IDP
#### Cojocaru Andrei Radu 341C4 + Ionescu Cristina 341C3

#### Descriere
Platforma noastra este dedicata comunitatii artistice si are ca scop facilitarea accesului la referinte vizuale utile in procesul creativ. Utilizatorii pot incarca imagini de referinta, pot asocia lucrarile proprii cu aceste referinte, si pot naviga continutul prin filtre si topicuri de interes. Platforma ofera si o componenta sociala, prin posibilitatea de a urmarii topicuri de interes, de a aprecia sau comenta lucrarile postate.

#### Comenzi si info utile
- frontend
    - ```npm run dev``` - porneste un server local de development oferit de Vite (este doar pentru development)
    - "in productie" cu docker se va face un build static cu ```npm run buil``` -- trebuie sa vedem ce inseamna asta mai exact 

- flux development
    - Browser (localhost:5173) → /api/... → proxy Vite → http://localhost:8000 (Kong) → microservicii
- flux productie
    - Browser (port 3000, Nginx container) → Kong Gateway → microservicii backend


- Rolare Docker Swarm
    - ```docker swarm init```
    - ```docker stack deploy -c docker-compose.yml artisthub```
