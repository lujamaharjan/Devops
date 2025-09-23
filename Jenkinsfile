pipeline {
    agent { label 'jenkins_slave' }

    environment {
        REGISTRY_URL = 'harbor.registry.local'
        IMAGE_NAME = 'todo-api'
        FRONTEND_IMAGE_NAME = 'todo-frontend'
        IMAGE_TAG = "v${BUILD_NUMBER}"
    }

    options {
        skipDefaultCheckout()
    }

    stages {

        stage('Checkout Code') {
            steps {
                git branch: 'master', url: 'https://github.com/lujamaharjan/Devops.git'
            }
        }

        stage('Build Docker Image') {
            steps {
                dir('app/backend') {
                    script {
                        sh """
                            docker build -t ${REGISTRY_URL}/sachin/${IMAGE_NAME}:${IMAGE_TAG} .
                            docker tag ${REGISTRY_URL}/sachin/${IMAGE_NAME}:${IMAGE_TAG} ${REGISTRY_URL}/sachin/${IMAGE_NAME}:latest
                        """
                    }
                }
            }
        }

        stage('Build Frontend') {
            steps {
                dir('app/frontend') {
                     sh """
                            docker build -t ${REGISTRY_URL}/sachin/${FRONTEND_IMAGE_NAME}:${IMAGE_TAG} .
                            docker tag ${REGISTRY_URL}/sachin/${IMAGE_NAME}:${IMAGE_TAG} ${REGISTRY_URL}/sachin/${IMAGE_NAME}:latest
                        """
                }
            }
        }

        stage('Login to Harbor') {
            steps {
                script {
                     withCredentials([usernamePassword(
                        credentialsId: 'HARBOR_CREDENTIALS',
                        usernameVariable: 'HARBOR_USER',
                        passwordVariable: 'HARBOR_PASS'
                    )]) {
                        sh "docker login ${REGISTRY_URL} -u $HARBOR_USER -p $HARBOR_PASS"
                        
                    }
                }
            }
        }

        stage('Push Docker Image') {
            steps {
                 script {
                    sh "docker push ${REGISTRY_URL}/sachin/${IMAGE_NAME}:${IMAGE_TAG}"
                    sh "docker push ${REGISTRY_URL}/sachin/${IMAGE_NAME}:latest"
                    sh "docker push ${REGISTRY_URL}/sachin/${FRONTEND_IMAGE_NAME}:${IMAGE_TAG}"
                    sh "docker push ${REGISTRY_URL}/sachin/${FRONTEND_IMAGE_NAME}:latest"
                }
            }
        }

        // stage('Scan Docker Images with Trivy') {
        //     steps {
        //         script {
        //             sh """
        //                 docker run --rm -v /var/run/docker.sock:/var/run/docker.sock aquasec/trivy image ${REGISTRY_URL}/sachin/${IMAGE_NAME}:${IMAGE_TAG} > trivy-backend-report.txt || true
        //                 docker run --rm -v /var/run/docker.sock:/var/run/docker.sock aquasec/trivy image ${REGISTRY_URL}/sachin/${FRONTEND_IMAGE_NAME}:${IMAGE_TAG} > trivy-frontend-report.txt || true
        //             """
        //             archiveArtifacts artifacts: 'trivy-*.txt', allowEmptyArchive: true
        //         }
        //     }
        // }

        // sqa_09b34b5e2b9c807f7b9ad9100753a45c5898b4cb
        stage('SonarQube Analysis') {
            agent { label 'jenkins_slave' }
            steps {
                withSonarQubeEnv('local-sonarqube') {
                    dir('app/backend') {
                        sh '''
                            dotnet tool install --global dotnet-sonarscanner || true
                            export PATH="$PATH:/root/.dotnet/tools"

                            dotnet sonarscanner begin \
                            /k:"todo-api" \
                            /n:"todo-api" \
                            /v:"${BUILD_NUMBER}" \
                            /d:sonar.host.url=$SONAR_HOST_URL \
                            /d:sonar.login=$SONAR_AUTH_TOKEN

                            dotnet build

                            dotnet sonarscanner end \
                            /d:sonar.login=$SONAR_AUTH_TOKEN
                        '''
                    }
                }
            }
        }
        
        // stage('Deploy to Swarm') {
        //     steps {
        //         withCredentials([usernamePassword(
        //             credentialsId: 'SWARM_SSH_CREDENTIALS',
        //             usernameVariable: 'SSH_USER',
        //             passwordVariable: 'SSH_PASS'
        //         )]) {
        //             sh """
        //                 sshpass -p "$SSH_PASS" ssh -o StrictHostKeyChecking=no $SSH_USER@192.168.56.108 \
        //                     "docker stack deploy -c /home/vagrant/docker-stack.yml todo_stack"
        //             """
        //         }
        //     }
        // }

        stage('Cleanup') {
            steps {
                script {
                    sh "docker rmi ${REGISTRY_URL}/sachin/${IMAGE_NAME}:${IMAGE_TAG} || true"
                    sh "docker rmi ${REGISTRY_URL}/sachin/${FRONTEND_IMAGE_NAME}:${IMAGE_TAG} || true"
                    sh "docker system prune -f"
                }
            }
        }
    }


    post {
        success {
            emailext(
                to: 'sachin.maharjan@dishhome.com.np',
                subject: "Build SUCCESS: ${env.JOB_NAME} #${env.BUILD_NUMBER}",
                body: "The build completed successfully.\nSee details at ${env.BUILD_URL}console"
            )
        }
        failure {
            emailext(
                to: 'sachin.maharjan@dishhome.com.np',
                subject: "Build FAILED: ${env.JOB_NAME} #${env.BUILD_NUMBER}",
                body: "The build failed.\nCheck logs at ${env.BUILD_URL}console"
            )
        }
    }
}
