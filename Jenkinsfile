pipeline {
    agent any
    tools
    {
    git 'Default'
    }
    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main' , url : 'https://github.com/HKGandhi/InventoryApp.git' // Replace with your repo
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build --no-restore'
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"'
                bat 'dotnet tool install --global dotnet-reportgenerator-globaltool'
                bat 'reportgenerator -reports:"**/TestResults/**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html'
            }
        }

        stage('Check Code Coverage') {
            steps {
                script {
                    def coverageFile = findFiles(glob: '**/TestResults/**/coverage.cobertura.xml')[0]
                    def coverageXml = new XmlSlurper().parse(coverageFile.path)
                    def lineRate = coverageXml.@line-rate.toDouble()
                    def lineCoveragePercent = lineRate * 100

                    echo "Line Coverage Achieved: ${lineCoveragePercent}%"

                    if (lineCoveragePercent < 50) {
                        error "❌ Code coverage too low: ${lineCoveragePercent}%. Minimum 50% required."
                    } else {
                        echo "✅ Code coverage is good: ${lineCoveragePercent}%"
                    }
                }
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish -c Release -o out'
            }
        }
    }

    post {
         always {

             publishHTML (target: [
                reportDir: 'coveragereport',
                reportFiles: 'index.html',
                reportName: 'Code Coverage Report'
            ]),
            publishCoverage adapters: [
                coberturaAdapter('**/TestResults/**/coverage.cobertura.xml')
            ],
            sourceFileResolver: sourceFiles('NEVER_STORE')
        }
        success {
            echo 'Build and publish succeeded!'
        }
        failure {
            echo 'Build failed!'
        }
    }
}
