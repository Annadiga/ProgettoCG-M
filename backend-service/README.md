# OPAS Backend Service
In questa cartella è presente il codice sorgente del servizio backend di OPAS, implementato in Javascript con Node.js.

## Comandi
- Se non è già installato, installare [Node.js](https://nodejs.org/it/download/)
- `npm install`
per installare le dipendenze (necessario solo la prima volta, richiede connessione ad internet)
- `npm start` per avviare il servizio sulla porta 3000

## Comandi per container Docker
In caso si preferisca utilizzare un container Docker invece che installare Node.js sul proprio computer, è possibile utilizzare i seguenti comandi:
- `docker build -t opas-backend .` per creare l'immagine Docker dal Dockerfile
- `docker run -p 3000:3000 opas-backend` per avviare il container ed esporre la porta 3000


## Configurazione rete locale
Per permettere l'accesso al servizio dall'app OPAS su HoloLens è necessario che il dispositivo e il computer su cui è in esecuzione il servizio backend si trovino sulla stessa rete locale e che il computer abbia indirizzo IP `192.168.137.1`.

Si consiglia di utilizzare la funzione Hotspot di Windows 10/11 per creare una rete locale WiFi alla quale far connettere l'Hololens, dato che Windows configurerà di default per se stesso l'indirizzo IP corretto.

Si tratta ovviamente di una limitazione legata al fatto che questa implementazione è solo a fini dimostrativi, nel caso reale il backend dovrebbe essere eseguito su un server accessibile dalla rete aziendale, protetto da un sistema di autenticazione.
