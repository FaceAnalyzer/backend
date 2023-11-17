build-docker:
	docker build -t plavy/faceanalyzer-backend:$(VERSION) -f Dockerfile .

deploy: build-docker
	docker push plavy/faceanalyzer-backend:$(VERSION)
	kubectl apply -n faceanalyzer -f manifest.yaml
	kubectl set image -n faceanalyzer deployment/faceanalyzer-backend faceanalyzer-backend=plavy/faceanalyzer-backend:$(VERSION)


migrate-deployment:
	dotnet ef migrations bundle --force -r linux-x64
	POD_NAME=$$(kubectl get pods -n faceanalyzer -l app=faceanalyzer-backend --no-headers -o custom-columns=":metadata.name"); \
	kubectl cp -n faceanalyzer ./efbundle $${POD_NAME}:.; \
	kubectl exec -n faceanalyzer $${POD_NAME} -- ./efbundle --connection 'server=mysql;user=root;password=root;database=face_analyzer'

migrate-local:
	dotnet ef migrations bundle --force -r linux-x64
	docker cp ./efbundle faceanalyzerapi-backend-1:/App
	docker exec -it faceanalyzerapi-backend-1 ./efbundle
