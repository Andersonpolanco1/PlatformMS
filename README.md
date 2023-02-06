>This documentation will be more detailed soon

## PlatformMS

There are currently two projects in this repository, Platforms Service and Commands Service.

Each one is responsible for storing their own information in their own database, they communicate asynchronously with RabbitMQ.

### Requirements to run this project:

You must have the following installed:
- [Docker Desktop](https://www.docker.com/products/docker-desktop/), after installing it you must activate kubernetes in the settings.
- [Visual Studio](https://visualstudio.microsoft.com/es/vs/) or [Visual Studio Code](https://code.visualstudio.com/)
- Some tool to test the API's like [Postman](https://www.postman.com/downloads/).
- [SQL Server Data Tools](https://www.microsoft.com/es-es/sql-server/developer-tools)

### Running the project:

After enabling Kubernetes on Docker Desktop...

Open a terminal and execute:

To deploy API Gateway - Ingress Nginx:

`kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.5.1/deploy/static/provider/cloud/deploy.yaml`

Generate the secret password in kubernetes for the Platforms Service database:

`kubectl create secret generic mssql --from-literal=SA_PASSWORD="pa55w0rd!"`

[You must modify the `Host` file](https://support.managed.com/kb/a683/how-to-modify-your-hosts-file-so-you-can-work-on-a-site-that-is-not-yet-live.aspx) to add a domain. in Windows the file path is `C:\Windows\System32\drivers\etc\hosts` for other operating systems, investigate the path, open the file and add the domain of your choice, in this project andersonpolanco.com.do was used as an example. do, you can use it or change it to your preference. Save the file and put the same domain in the `ingress-srv.yaml` file

Then go to the folder in the `PlatformsMS/K8S` path and run the following commands:

`kubectl apply -f ingress-srv.yaml`

`kubectl apply -f local-pvc.yaml`

`kubectl apply -f platforms-np-srv.yaml`

`kubectl apply -f rabbitmq-depl.yaml`

Go to the path `PlatformMS/PlatformsService/K8S` and run the following commands:

In the file platforms-depl.yaml correct the name of the image you just created

And then execute:

`kubectl apply -f platforms-mssql-depl.yaml`

`kubectl apply -f platforms-depl.yaml`

Go to the path /PlatformMS/CommandService/K8S and run the following commands:

`kubectl apply -f commands-depl.yaml`

`kubectl apply -f commands-mssql-depl.yaml`

`kubectl apply -f commands-mssql-pvc.yaml`

Services have their migrations and run them automatically the first time they are run, Platforms Service has seeded data.

Test the endpoints.




