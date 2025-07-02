pipeline {
    agent { label 'jenkins_slave' }

    environment {
        REGISTRY_URL = 'harbor.registry.local'
        IMAGE_NAME = 'todo-api'
        IMAGE_TAG = "v${BUILD_NUMBER}"
        HARBOR_CREDENTIALS = credentials(' HARBOR_CREDENTIALS') // Jenkins credential ID
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
                        """
                    }
                }
            }
        }

        stage('Login to Harbor') {
            steps {
                script {
                    sh """
                       docker login ${REGISTRY_URL} -u admin -p Harbor12345
                    """
                }
            }
        }

        stage('Push Docker Image') {
            steps {
                script {
                    sh """
                        docker push ${REGISTRY_URL}/sachin/${IMAGE_NAME}:${IMAGE_TAG}
                    """
                }
            }
        }

        stage('Cleanup') {
            steps {
                script {
                    sh """
                        docker rmi ${REGISTRY_URL}/sachin/${IMAGE_NAME}:${IMAGE_TAG} || true
                    """
                }
            }
        }
    }

    post {
        failure {
            echo "Build failed!"
        }
        success {
            echo "Image pushed: ${REGISTRY_URL}/sachin/${IMAGE_NAME}:${IMAGE_TAG}"
        }
    }
}
