// Define um componente React para o conteúdo principal
function Content() {
    // Função para inicializar o mapa
    function initMap() {
        // Coordenadas do centro do mapa
        var center = { lat: -23.5505, lng: -46.6333 };

        // Opções do mapa
        var mapOptions = {
            zoom: 10,
            center: center
        };

        // Criação do mapa
        var map = new google.maps.Map(document.getElementById('map'), mapOptions);

        // Obter a localização do usuário
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var userLatLng = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };

                // Adicionar marcador para a localização do usuário
                var userMarker = new google.maps.Marker({
                    position: userLatLng,
                    map: map,
                    title: 'Sua localização'
                });

                // Centralizar o mapa na localização do usuário
                map.setCenter(userLatLng);
            }, function () {
                console.error('Erro ao obter a localização do usuário.');
            });
        } else {
            console.error('Navegador não suporta geolocalização.');
        }
    }

    // Chama a função para inicializar o mapa quando o componente for montado
    React.useEffect(() => {
        initMap();
    }, []);

    return React.createElement(
        'div',
        { className: 'content' },
        React.createElement('div', { id: 'map', style: { height: '400px' } })
    );
}

// Renderiza o componente Menu e Content no elemento com o id "root"
ReactDOM.render(React.createElement(
    'div',
    null,
    React.createElement(Content, null)
), document.getElementById('root'));